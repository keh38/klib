using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using KLib.MSGraph.Data;
using KLib.Utilities;

namespace KLib.MSGraph
{
    public class GraphClient
    {
        // Set the API Endpoint to Graph 'me' endpoint. 
        // To change from Microsoft public cloud to a national cloud, use another value of graphAPIEndpoint.
        // Reference with Graph endpoints here: https://docs.microsoft.com/graph/deployments#microsoft-graph-and-graph-explorer-service-root-endpoints
        static string _graphAPIEndpoint = "https://graph.microsoft.com/v1.0/me";

        // Set the scope for API call to user.read
        static string[] _scopes = new string[] { "user.read", "files.readwrite", "sites.readwrite.all" };

        AuthenticationResult _authResult = null;
        IPublicClientApplication _app = null;
        string _acctInfoPath;
        string _basePath = "";

        public GraphClient()
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;
            _acctInfoPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), ".msalacct.xml");

            GraphApp.CreateApplication(true);
            _app = GraphApp.PublicClientApp;
        }

        public bool IsConnected { get { return _authResult != null; } }
        public bool IsReady { get { return _authResult != null && _basePath != null; } }

        public bool AcquireAuthorizationToken()
        {
            var thread = AcquireAuthorizationTokenAsync();
            thread.Wait();

            if (thread.IsCompleted)
                return thread.Result;

            return false;
        }

        public bool SignInUser()
        {
            var thread = SignInUserAsync();
            thread.Wait();

            if (thread.IsCompleted)
                return thread.Result;

            return false;
        }

        public bool SignOutUser()
        {
            var thread = SignOutUserAsync();
            thread.Wait();

            if (thread.IsCompleted)
                return thread.Result;

            return false;
        }

        public bool ClearLoginCache()
        {
            File.Delete(TokenCacheHelper.CacheFilePath);
            File.Delete(_acctInfoPath);
            return true;
        }

        public string GetToken()
        {
            return _authResult.AccessToken;
        }

        // See the following. In this case, waiting for the task causes it to lock up. Using the method shown here causes the async op to run
        // in a separate thread. Note that the optimal solution is to do everything using async/await, but we *cannot* await from Unity. Unity
        // requires synchronous calls; we just have to find reliable sub-optimal solutions.
        //
        // http://blogs.msdn.com/b/pfxteam/archive/2012/04/13/10293638.aspx
        // 
        public string GetHttpContent(string cmd)
        {
            return Task.Run(() => GetHttpContentAsync(cmd)).Result;
        }

        public string PutHttpContent(string cmd, byte[] content)
        {
            return Task.Run(() => PutHttpContentAsync(cmd, content)).Result;
        }

        public string PostHttpContent(string cmd, string content)
        {
            return Task.Run(() => PostHttpContentAsync(cmd, content)).Result;
        }

        public bool Upload(string remoteFolder, string localPath)
        {
            return Task.Run(() => UploadFile(remoteFolder, localPath)).Result;
        }

        public string DeleteHttp(string cmd)
        {
            return Task.Run(() => DeleteHttpAsync(cmd)).Result;
        }

        public bool DoesFileExist(string path)
        {
            return Task.Run(() => FileExists(path)).Result;
        }

        public bool SetBaseFolderSync(string path)
        {
            return Task.Run(() => SetBaseFolder(path)).Result;
        }

        public async Task<bool> AcquireAuthorizationTokenAsync()
        {
            _authResult = null;
            try
            {
                if (File.Exists(_acctInfoPath))
                {
                    var acct = FileIO.XmlDeserialize<AccountInfo>(_acctInfoPath);
                    var a = new MsalAccount(acct.ObjectID, acct.TokenID);

                    _authResult = await _app.AcquireTokenSilent(_scopes, a)
                        .ExecuteAsync();
                }
            }
            catch (MsalUiRequiredException ex)
            {
            }
            catch (Exception ex)
            {
            }

            return _authResult != null;
        }

        public async Task<bool> SignInUserAsync()
        {
            _authResult = null;

            //try
            {
                _authResult = await _app.AcquireTokenInteractive(_scopes)
                    .WithAccount(null)
                    .WithPrompt(Prompt.SelectAccount)
                    .ExecuteAsync();

                var account = new AccountInfo(_authResult.UniqueId, _authResult.TenantId);
                FileIO.XmlSerialize(account, _acctInfoPath);
            }
            //catch (MsalException msalex)
            //{
            //}
            //catch (Exception ex)
            //{
            //}

            return _authResult != null;
        }

        public async Task<bool> SignOutUserAsync()
        {
            bool signedOut = false;

            var accounts = await _app.GetAccountsAsync();
            if (accounts.Any())
            {
                try
                {
                    await _app.RemoveAsync(accounts.FirstOrDefault());
                    signedOut = true;
                    _authResult = null;
                    _basePath = "";
                }
                catch (MsalException ex)
                {
                }
            }

            return signedOut;
        }

        public async Task<bool> Connect()
        {
            var signedIn = await AcquireAuthorizationTokenAsync();
            if (!signedIn)
            {
                signedIn = await SignInUserAsync();
            }

            return signedIn;
        }

        public async Task<bool> SetBaseFolder(string name)
        {
            _basePath = "";

            DriveItem di = null;

            var ui = await GetUserInfo();

            if (ui.userPrincipalName.ToLower().Contains("hancock"))
            {
                string cmd = "/drive/root/children?select=id,name,folder,parentReference";
                var result = await GetHttpContentAsync(cmd);

                var items = JsonConvert.DeserializeObject<DriveItemContainer>(result);
                di = items.value.Find(o => o.folder != null && o.name == name);
            }

            if (di != null)
            {
                _basePath = $"/drives/{di.parentReference.driveId}/items/{di.id}";
            }
            else
            {
                string cmd = "/drive/sharedWithMe";
                var result = await GetHttpContentAsync(cmd);

                result = result.Replace("@microsoft.graph.downloadUrl", "url");
                var items = JsonConvert.DeserializeObject<DriveItemContainer>(result);

                di = items.value.Find(o => o.folder != null && o.name == name);
                if (di != null)
                {
                    _basePath = $"/drives/{di.remoteItem.parentReference.driveId}/items/{di.remoteItem.id}";
                }
            }

            Debug.WriteLine("BASEPATH: " + _basePath);
            return !string.IsNullOrEmpty(_basePath);
        }

        public async Task<UserInfo> GetUserInfo()
        {
            var result = await GetHttpContentAsync("");

            var ui = JsonConvert.DeserializeObject<UserInfo>(result);
            return ui;
        }

        public async Task<DriveItem> GetItem(string itemPath)
        {
            DriveItem item = null;
            string url = _basePath;
            if (!string.IsNullOrEmpty(itemPath))
            {
                url += $":/{itemPath}:";
            }
            try
            {
                var result = await GetHttpContentAsync(url);

                result = result.Replace("@microsoft.graph.downloadUrl", "url");
                item = JsonConvert.DeserializeObject<DriveItem>(result);
            }
            catch (System.Exception ex)
            {
                if (!ex.Message.StartsWith("404"))
                    throw (ex);
            }

            return item;
        }

        public async Task<List<DriveItem>> GetItems(string folder)
        {
            List<DriveItem> driveItems = new List<DriveItem>();

            string url = _basePath;
            if (!string.IsNullOrEmpty(folder))
            {
                url += $":/{folder}:";
            }
            url += "/children?select=name,@microsoft.graph.downloadUrl,folder,fileSystemInfo,parentReference,@odata.nextLink";

            while (!string.IsNullOrEmpty(url))
            {
                url = Uri.UnescapeDataString(url);

                var result = await GetHttpContentAsync(url);

                result = result.Replace("@microsoft.graph.downloadUrl", "url");
                result = result.Replace("@odata.nextLink", "nextLink");

                var items = JsonConvert.DeserializeObject<DriveItemContainer>(result);
                driveItems.AddRange(items.value);

                url = items.nextLink;
            }

            return driveItems;
        }

        public async Task<List<string>> GetItemNames(string folder)
        {
            var items = await GetItems(folder);
            return items.Select(o => o.name).ToList();
        }

        public async Task<List<DriveItem>> GetFiles(string folder)
        {
            var items = await GetItems(folder);
            return items.FindAll(o => o.folder == null);
        }

        public async Task<List<DriveItem>> GetFolders(string folder)
        {
            var items = await GetItems(folder);
            return items.FindAll(o => o.folder != null);
        }

        public async Task<List<string>> GetFolderNames(string folder)
        {
            var items = await GetFolders(folder);
            return items.FindAll(o => o.folder != null).Select(o => o.name).ToList();
        }

        public async Task<bool> FileExists(string path)
        {
            var item = await GetItem(path);
            return item != null;
        }

        public async Task<bool> FolderExists(string folderName)
        {
            string parent = "";
            string child = folderName;
            if (folderName.Contains("/"))
            {
                int lastSlash = folderName.LastIndexOf('/');
                parent = folderName.Substring(0, lastSlash);
                child = folderName.Substring(lastSlash + 1);
            }

            var folders = await GetFolderNames(parent);
            return folders != null ? folders.Contains(child) : false;
        }

        public async Task<bool> CreateFolder(string folderName)
        {
            string parent = "";
            string child = folderName;
            if (folderName.Contains("/"))
            {
                int lastSlash = folderName.LastIndexOf('/');
                parent = folderName.Substring(0, lastSlash);
                child = folderName.Substring(lastSlash + 1);
            }

            string cmd = _basePath;
            if (!string.IsNullOrEmpty(parent))
            {
                cmd += $":/{parent}:";
            }
            cmd += "/children";

            string content = $"{{\"name\": \"{child}\", \"folder\": {{ }} }}";

            var result = await PostHttpContentAsync(cmd, content);

            return result != null;
        }

        public async Task<bool> UploadFile(string remoteFolder, string localPath)
        {
            var fn = Path.GetFileName(localPath);
            string cmd = $"{_basePath}:/{remoteFolder}/{fn}";

            var content = File.ReadAllBytes(localPath);
            var result = await PutHttpContentAsync($"{cmd}:/content", content);

            Data.FileSystemInfo fileSystemInfo = new Data.FileSystemInfo();
            fileSystemInfo.createdDateTime = File.GetCreationTimeUtc(localPath).ToString("s") + "Z";
            fileSystemInfo.lastModifiedDateTime = File.GetLastWriteTimeUtc(localPath).ToString("s") + "Z";

            var body = $"{{\"fileSystemInfo\":{JsonConvert.SerializeObject(fileSystemInfo)}}}";
            result = await PatchHttpContentAsync(cmd, body);

            return result != null;
        }

        public async Task<bool> UploadFile(string remoteFolder, string filename, string content)
        {
            string cmd = $"{_basePath}:/{remoteFolder}/{filename}";

            var result = await PutHttpContentAsync($"{cmd}:/content", Encoding.ASCII.GetBytes(content));

            return result != null;
        }

        public async Task<bool> MoveFile(DriveItem item, string remoteFolder)
        {
            string cmd = $"{item.parentReference.path}/{item.name}";

            var body = "{";
            body += "\"parentReference\":{";
            body += $"\"path\":\"{remoteFolder}\"";
            body += "}, \"name\": \"" + item.name + "\"}";
            return await PatchHttpContentAsync(cmd, body) != null;
        }

        public async Task<bool> DeleteFile(string remoteFolder, string filename)
        {
            string cmd = $"{_basePath}:/{remoteFolder}/{filename}";
            return await DeleteHttpAsync(cmd) != null;
        }

        public async Task<bool> DeleteFile(DriveItem item)
        {
            string cmd = $"{item.parentReference.path}/{item.name}";
            return await DeleteHttpAsync(cmd) != null;
        }

        public async Task<bool> DownloadFileByPath(string remotePath, string localPath)
        {
            bool success = false;

            var item = await GetItem(remotePath);
            if (item != null)
            {
                success = DownloadFile(item.url, localPath);
            }
            return success;
        }

        public bool DownloadFile(DriveItem item, string localPath)
        {
            bool success = false;

            success = DownloadFile(item.url, localPath);

            var tcreated = System.DateTime.Parse(item.fileSystemInfo.createdDateTime).ToUniversalTime();
            var tmodified = System.DateTime.Parse(item.fileSystemInfo.lastModifiedDateTime).ToUniversalTime();

            File.SetCreationTimeUtc(localPath, tcreated);
            File.SetLastWriteTimeUtc(localPath, tmodified);

            return success;
        }


        private WebClient _client;
        public void StartDownload()
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;
            _client = new WebClient();
            _client.Proxy = null;
        }

        public bool ContinueDownload(string remotePath, string localPath)
        {
            bool success = false;
            int ntries = 0;
            int maxtries = 5;

            while (ntries < maxtries && !success)
            {
                try
                {
                    _client.DownloadFile(remotePath, localPath);
                    success = true;
                }
                catch (Exception ex)
                {
                    ntries++;
                }
            }
            return success;
        }

        public void EndDownload()
        {
            _client.Dispose();
            _client = null;
        }

        public bool DownloadFile(string remotePath, string localPath)
        {
            bool success = false;
            int ntries = 0;
            int maxtries = 5;

            while (ntries < maxtries && !success)
            {
                try
                {
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;
                    using (var client = new WebClient())
                    {
                        client.Proxy = null;
                        client.DownloadFile(remotePath, localPath);
                        success = true;
                    }
                }
                catch (Exception ex)
                {
                    ntries++;
                }
            }
            return success;
        }

        public bool DownloadFile2(string remotePath, string localPath)
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            using (var client = new HttpClient())
            {
                //client.DefaultRequestHeaders.ConnectionClose = false;
                using (var s = client.GetStreamAsync(remotePath))
                {
                    using (var fs = new FileStream(localPath, FileMode.OpenOrCreate))
                    {
                        s.Result.CopyTo(fs);
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Perform an HTTP GET request to a URL using an HTTP Authorization header
        /// </summary>
        /// <param name="url">The URL</param>
        /// <param name="token">The token</param>
        /// <returns>String containing the results of the GET operation</returns>
        private async Task<string> GetHttpContentAsync(string cmd)
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;

            string url = cmd;
            if (!url.StartsWith(_graphAPIEndpoint)) url = _graphAPIEndpoint + url;

            var httpClient = new HttpClient();
            HttpResponseMessage response;
            string content = null;
            bool finished = false;
            Error error = null;
            bool allowRetry = true;

            while (!finished)
            {
                try
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, url);

                    //Add the token in Authorization header
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authResult.AccessToken);
                    response = await httpClient.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        content = await response.Content.ReadAsStringAsync();
                        error = CheckForError(content);
                    }
                    else
                    {
                        error = new Error(((int)(response.StatusCode)).ToString(), response.StatusCode.ToString());
                    }
                }
                catch (Exception ex)
                {
                    error = new Error("MSGraphError", ex.ToString());
                }

                if (error != null && allowRetry && (error.code == "InvalidAuthenticationToken" || error.code == "401"))
                {
                    error = null;
                    System.Diagnostics.Debug.WriteLine(DateTime.Now.ToLongDateString() + ": refreshing token");
                    var refreshed = await AcquireAuthorizationTokenAsync();
                    if (!refreshed)
                    {
                        error = new Error("InvalidAuthenticationToken", "failed to refresh token");
                        System.Diagnostics.Debug.WriteLine("failed to refresh token");
                        finished = true;
                    }
                }
                else
                {
                    finished = true;
                }
            }

            if (error != null)
            {
                throw new Exception($"{error.code}: {error.message}");
            }

            return content;
        }

        private async Task<byte[]> GetHttpContentAsBytesAsync(string cmd)
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;

            string url = _graphAPIEndpoint + cmd;

            var httpClient = new HttpClient();
            HttpResponseMessage response;
            byte[] content = null;
            bool finished = false;
            Error error = null;
            bool allowRetry = true;

            while (!finished)
            {
                try
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, url);
                    //Add the token in Authorization header
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authResult.AccessToken);
                    response = await httpClient.SendAsync(request);
                    content = await response.Content.ReadAsByteArrayAsync();
                    error = CheckForError(content);
                }
                catch (Exception ex)
                {
                    error = new Error("MSGraphError", ex.ToString());
                }

                if (error != null && allowRetry && (error.code == "InvalidAuthenticationToken" || error.code == "401"))
                {
                    var refreshed = await AcquireAuthorizationTokenAsync();
                    if (!refreshed)
                    {
                        error = new Error("InvalidAuthenticationToken", "failed to refresh token");
                        finished = true;
                    }
                }
                else
                {
                    finished = true;
                }
            }

            if (error != null)
            {
                throw new Exception($"{error.code}: {error.message}");
            }

            return content;
        }

        private async Task<string> PutHttpContentAsync(string cmd, byte[] content)
        {
            string url = _graphAPIEndpoint + cmd;
            var httpClient = new HttpClient();
            HttpResponseMessage response;
            bool finished = false;
            string result = null;
            Error error = null;
            bool allowRetry = true;

            while (!finished)
            {
                try
                {
                    var request = new HttpRequestMessage(HttpMethod.Put, url);

                    //Add the token in Authorization header
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authResult.AccessToken);
                    request.Content = new ByteArrayContent(content);

                    response = await httpClient.SendAsync(request);
                    result = await response.Content.ReadAsStringAsync();
                    error = CheckForError(result);
                }
                catch (Exception ex)
                {
                    error = new Error("MSGraphError", ex.ToString());
                }

                if (error != null && allowRetry && (error.code == "InvalidAuthenticationToken" || error.code == "401"))
                {
                    var refreshed = await AcquireAuthorizationTokenAsync();
                    if (!refreshed)
                    {
                        error = new Error("InvalidAuthenticationToken", "failed to refresh token");
                        finished = true;
                    }
                }
                else
                {
                    finished = true;
                }
            }

            if (error != null)
            {
                throw new Exception($"{error.code}: {error.message}");
            }

            return result;
        }

        private async Task<string> PostHttpContentAsync(string cmd, string content)
        {
            string url = _graphAPIEndpoint + cmd;
            var httpClient = new HttpClient();
            HttpResponseMessage response;
            bool finished = false;
            string result = null;
            Error error = null;
            bool allowRetry = true;

            while (!finished)
            {
                try
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, url);

                    //Add the token in Authorization header
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authResult.AccessToken);
                    request.Content = new StringContent(content, Encoding.UTF8, "application/json");
                    response = await httpClient.SendAsync(request);
                    result = await response.Content.ReadAsStringAsync();
                    error = CheckForError(result);
                }
                catch (Exception ex)
                {
                    error = new Error("MSGraphError", ex.ToString());
                }

                if (error != null && allowRetry && (error.code == "InvalidAuthenticationToken" || error.code == "401"))
                {
                    var refreshed = await AcquireAuthorizationTokenAsync();
                    if (!refreshed)
                    {
                        error = new Error("InvalidAuthenticationToken", "failed to refresh token");
                        finished = true;
                    }
                }
                else
                {
                    finished = true;
                }
            }

            if (error != null)
            {
                throw new Exception($"{error.code}: {error.message}");
            }

            return result;
        }

        private async Task<string> PatchHttpContentAsync(string cmd, string content)
        {
            string url = _graphAPIEndpoint + cmd;
            var httpClient = new HttpClient();
            HttpResponseMessage response;
            bool finished = false;
            string result = null;
            Error error = null;
            bool allowRetry = true;
            
            while (!finished)
            {
                try
                {
                    var method = new HttpMethod("PATCH");
                    var request = new HttpRequestMessage(method, url);

                    //Add the token in Authorization header
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authResult.AccessToken);
                    request.Content = new StringContent(content, Encoding.UTF8, "application/json");
                    response = await httpClient.SendAsync(request);
                    result = await response.Content.ReadAsStringAsync();
                    error = CheckForError(result);
                }
                catch (Exception ex)
                {
                    error = new Error("MSGraphError", ex.ToString());
                }

                if (error != null && allowRetry && (error.code == "InvalidAuthenticationToken" || error.code == "401"))
                {
                    var refreshed = await AcquireAuthorizationTokenAsync();
                    if (!refreshed)
                    {
                        error = new Error("InvalidAuthenticationToken", "failed to refresh token");
                        finished = true;
                    }
                }
                else
                {
                    finished = true;
                }
            }

            if (error != null)
            {
                throw new Exception($"{error.code}: {error.message}");
            }

            return result;
        }

        private async Task<string> DeleteHttpAsync(string cmd)
        {
            string url = _graphAPIEndpoint + cmd;
            var httpClient = new HttpClient();
            HttpResponseMessage response;
            bool finished = false;
            string result = null;
            Error error = null;
            bool allowRetry = true;

            while (!finished)
            {
                try
                {
                    var method = new HttpMethod("DELETE");
                    var request = new HttpRequestMessage(method, url);

                    //Add the token in Authorization header
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authResult.AccessToken);
                    response = await httpClient.SendAsync(request);
                    result = await response.Content.ReadAsStringAsync();
                    error = CheckForError(result);
                }
                catch (Exception ex)
                {
                    error = new Error("MSGraphError", ex.ToString());
                }

                if (error != null && allowRetry && (error.code == "InvalidAuthenticationToken" || error.code == "401"))
                {
                    var refreshed = await AcquireAuthorizationTokenAsync();
                    if (!refreshed)
                    {
                        error = new Error("InvalidAuthenticationToken", "failed to refresh token");
                        finished = true;
                    }
                }
                else
                {
                    finished = true;
                }
            }

            if (error != null)
            {
                throw new Exception($"{error.code}: {error.message}");
            }

            return result;
        }

        private Error CheckForError(string result)
        {
            Error error = null;
            if (result.StartsWith("{\"error"))
            {
                error = JsonConvert.DeserializeObject<ErrorResource>(result).error;
            }
            return error;
        }

        private Error CheckForError(byte[] result)
        {
            string stringStart = Encoding.Default.GetString(result, 0, 10);

            Error error = null;
            if (stringStart.StartsWith("{\"error"))
            {
                string wholeString = Encoding.Default.GetString(result);
                error = JsonConvert.DeserializeObject<ErrorResource>(wholeString).error;
            }
            return error;
        }

    }

}