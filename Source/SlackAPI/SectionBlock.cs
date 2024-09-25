using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLib.SlackAPI
{
    public class SectionBlock : Block
    {
        public TextElement text = null;

        public SectionBlock() { type = "section"; }
        public SectionBlock(string type, string text)
        {
            this.type = "section";
            this.text = new TextElement(type, text);
        }

    }
}
