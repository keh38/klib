using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLib.Net
{
    public class ARPTable
    {
        public string HostAddress { get; private set; }
        public List<IPInfo> ClientInfo { get; private set; }

        public ARPTable(string hostAddress)
        {
            HostAddress = hostAddress;
            ClientInfo = new List<IPInfo>();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine(HostAddress);
            foreach (var ci in ClientInfo)
            {
                sb.AppendLine($"{ci.IPAddress}: {ci.Type}");
            }

            return sb.ToString();
        }

    }
}
