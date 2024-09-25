using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLib.SlackAPI
{
    public class AccessTokenResponse
    {
        public string app_id;
        public AuthedUser authed_user = new AuthedUser();
        public string scope;
        public string token_type;
        public string access_token;
        public string bot_user_id;
        public Team team = new Team();
    }
}
