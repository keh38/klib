using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLib.SlackAPI
{
    public class TextElement
    {
        public string type = "plain_text";
        public string text = "";

        public TextElement(string type, string text)
        {
            this.type = type;
            this.text = text;
        }
    }
}
