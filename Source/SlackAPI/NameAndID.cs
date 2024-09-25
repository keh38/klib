using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLib.SlackAPI
{
    public class NameAndID
    {
        public string id = "";
        public string name = "";

        public NameAndID() { }
        public NameAndID(string name, string id)
        {
            this.name = name;
            this.id = id;
        }
    }
}
