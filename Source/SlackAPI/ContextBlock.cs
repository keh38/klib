using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLib.SlackAPI
{
    public class ContextBlock : Block
    {
        public List<TextElement> elements = new List<TextElement>();

        public ContextBlock()
        {
            this.type = "context";
        }
        public ContextBlock(TextElement element)
        {
            this.type = "context";
            elements.Add(element);
        }
    }
}
