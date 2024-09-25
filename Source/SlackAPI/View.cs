using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLib.SlackAPI
{
    public class View
    {
        public string type = "home";
        public List<Block> blocks;

        public View() { }
        public View(List<Block> blocks)
        {
            this.blocks = blocks;
        }
    }
}
