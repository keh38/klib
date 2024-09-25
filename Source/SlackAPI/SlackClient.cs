using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace KLib.SlackAPI
{
    public class SlackClient
    {
        private string _baseURL = "https://slack.com/api/";
        private string _redirectHost = "127.0.0.1";
        private int _redirectPort = 5000;

        public SlackClient()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        public AccessTokenResponse GetAccessToken(string clientID, string clientSecret, string scope)
        {
            string oauthCode = GetOAuthCode(clientID, scope);

            Uri uri = GetSlackUri(
                "oauth.v2.access",
                new Tuple<string, string>("code", oauthCode),
                new Tuple<string, string>("client_id", clientID),
                new Tuple<string, string>("client_secret", clientSecret)
                );

            return APIRequest<AccessTokenResponse>(uri);
        }

        public User GetUserInfo(string token, string id)
        {
            Uri uri = GetSlackUri(
                "users.info",
                new Tuple<string, string>("token", token),
                new Tuple<string, string>("user", id)
                );

            var usersInfoResponse = APIRequest<UsersInfoResponse>(uri);
            return usersInfoResponse.user;
        }

        public List<Channel> GetUserConversations(string token, string id)
        {
            Uri uri = GetSlackUri(
                "users.conversations",
                new Tuple<string, string>("token", token),
                new Tuple<string, string>("user", id)
                );

            var usersConversationsResponse = APIRequest<UsersConversationsResponse>(uri);

            List<Channel> channels = new List<Channel>();
            foreach (Channel ch in usersConversationsResponse.channels)
                if (ch.is_channel) channels.Add(ch);

            return channels;
        }

        public List<string> GetConversationMembers(string token, string channel)
        {
            Uri uri = GetSlackUri(
                "conversations.members",
                new Tuple<string, string>("token", token),
                new Tuple<string, string>("channel", channel)
                );

            return APIRequest<ConversationsMembersResponse>(uri).members;
        }

        public void PostMessage(string token, string channel, string text, string username)
        {
            Uri uri = GetSlackUri(
                "chat.postMessage",
                new Tuple<string, string>("mrkdwn", "true")
                );

            Message msg = new Message(token, channel, text, username);

            APIRequestInBody(uri, token, msg);
        }

        public void PostFormattedMessage(string token, string channel, string text, List<Block> blocks, string username)
        {
            Uri uri = GetSlackUri("chat.postMessage");
            FormattedMessage msg = new FormattedMessage(token, channel, text, JsonConvert.SerializeObject(blocks), username);

            APIRequestInBody(uri, token, msg);
        }

        public void PublishView(string token, string user_id, List<Block> blocks)
        {
            Uri uri = GetSlackUri("views.publish");
            Publish p = new Publish(token, user_id, JsonConvert.SerializeObject(new View(blocks)));
 
            APIRequestInBody(uri, token, p);
        }

        public void UploadTextFile(string token, string user_id, string comment, string name, string content)
        {
            Uri uri = GetSlackUri(
                "files.upload",
                new Tuple<string, string>("token", token),
                new Tuple<string, string>("channels", user_id),
                new Tuple<string, string>("initial_comment", comment),
                new Tuple<string, string>("filename", name),
                new Tuple<string, string>("content", content)
                );
            APIRequest<DefaultResponse>(uri);
        }

        private string GetOAuthCode(string clientID, string scope)
        {
            var uri = GetAuthorizeUri(clientID, scope);
            Process.Start(uri.ToString());

            string response = ReceiveRedirection();

            Match m = Regex.Match(response, @"\?error=([\d.a-z_]+)&");
            if (m.Success)
            {
                throw new Exception("Error connecting to Slack: " + m.Groups[1].Value);
            }

            m = Regex.Match(response, @"\?code=([\d.a-z]+)&");
            if (!m.Success)
            {
                throw new Exception("Error connecting to Slack");
            }

            return m.Groups[1].Value;
        }

        private Uri GetAuthorizeUri(string clientID, string scope)
        {
            var builder = new StringBuilder("https://slack.com/oauth/v2/");

            builder.Append("authorize?");
            builder.Append("client_id=" + Uri.EscapeDataString(clientID));

            builder.Append("&");
            builder.Append("scope=" + Uri.EscapeDataString(scope));

            builder.Append("&");
            builder.Append("redirect_uri=https://localhost:" + _redirectPort);

            return new Uri(builder.ToString());
        }

        private Uri GetSlackUri(string method, params Tuple<string, string>[] parameters)
        {
            var builder = new StringBuilder(_baseURL);

            builder.Append(method);
            if (parameters.Length > 0)
            {
                builder.Append("?");
                for (int k = 0; k < parameters.Length; k++)
                {
                    if (k > 0) builder.Append("&");
                    builder.Append(string.Format("{0}={1}", Uri.EscapeDataString(parameters[k].Item1), Uri.EscapeDataString(parameters[k].Item2)));
                }
            }
            return new Uri(builder.ToString());
        }

        private T APIRequest<T>(Uri uri)
        {
            string body = "";
            var request = HttpWebRequest.Create(uri);

            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                var oResponse = new StreamReader(response.GetResponseStream());
                body = oResponse.ReadToEnd();
                oResponse.Close();
            }

            return JsonConvert.DeserializeObject<T>(body);
        }

        private void APIRequestInBody<T>(Uri uri, string token, T requestBody)
        {
            string responseBody = "";
            var request = HttpWebRequest.Create(uri);

            request.ContentType = "application/json";
            request.Headers["Authorization"] = "Bearer " + token;
            request.Method = "POST";

            var oRequest = new StreamWriter(request.GetRequestStream());
            oRequest.WriteLine(JsonConvert.SerializeObject(requestBody));
            oRequest.Close();

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                var oResponse = new StreamReader(response.GetResponseStream());
                responseBody = oResponse.ReadToEnd();
                oResponse.Close();
            }

            var r = JsonConvert.DeserializeObject<DefaultResponse>(responseBody);
            if (!r.ok)
            {
                throw new Exception("Slack error: " + r.error);
            }

        }

        private string ReceiveRedirection()
        {
            string response = "";
            var _listener = new TcpListener(IPAddress.Parse(_redirectHost), _redirectPort);
            _listener.Start();

            int timeout_ms = 60000;
            int sleep_ms = 10;
            int maxTries = timeout_ms / sleep_ms;
            int ntries = 0;

            while (!_listener.Pending())
            {
                Thread.Sleep(sleep_ms);
                if (++ntries == maxTries)
                    throw new Exception("Timed out waiting for Slack");
            }

            TcpClient client = _listener.AcceptTcpClient();

            using (NetworkStream stream = client.GetStream())
            using (BinaryWriter theWriter = new BinaryWriter(stream))
            using (StreamReader theReader = new StreamReader(stream))
            {
                response = theReader.ReadLine();
                theWriter.Write("HTTP/1.1 200 OK");
                theWriter.Flush();
                Thread.Sleep(1000);
            }

            client.Close();

            return response;
        }



    }
}
