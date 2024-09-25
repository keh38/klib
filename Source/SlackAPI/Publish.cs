using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLib.SlackAPI
{
    public class Publish
    {
        public string token;
        public string user_id;
        public string view;

        public Publish() { }
        public Publish(string token, string user_id, string view)
        {
            this.token = token;
            this.user_id = user_id;
            this.view = view;
        }
    }
}
