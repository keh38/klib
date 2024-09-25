using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLib.SlackAPI
{
    public class FormattedMessage : Message
    {
        public string blocks = "";
        public FormattedMessage() { }
        public FormattedMessage(string token, string channel, string text, string blocks, string username) : base(token, channel, text, username)
        {
            this.blocks = blocks;
        }

    }
}
