using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLib.SlackAPI
{
    public class Message
    {
        public string token;
        public string channel;
        public string text;
        public string username;

        public Message() { }
        public Message(string token, string channel, string text, string username)
        {
            this.token = token;
            this.channel = channel;
            this.text = text;
            this.username = username;
        }
    }
}
