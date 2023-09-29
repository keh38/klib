using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace KLib.Net
{
    public class IPInfo
    {
        public IPInfo(string ipAddress, string macAddress, string type)
        {
            this.IPAddress = ipAddress;
            this.MacAddress = macAddress;
            this.Type = type;
        }

        public string MacAddress { get; private set; }
        public string IPAddress { get; private set; }
        public string Type { get; private set; }

        private string _HostName = string.Empty;
        public string HostName
        {
            get
            {
                if (string.IsNullOrEmpty(this._HostName))
                {
                    try
                    {
                        // Retrieve the "Host Name" for this IP Address. This is the "Name" of the machine.
                        this._HostName = Dns.GetHostEntry(this.IPAddress).HostName;
                    }
                    catch
                    {
                        this._HostName = string.Empty;
                    }
                }
                return this._HostName;
            }
        }
    }
}
