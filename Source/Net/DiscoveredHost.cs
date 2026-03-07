using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLib.Net
{

    // -------------------------------------------------------------------------
    // DiscoveredHost
    // Wraps a ServerBeacon with the timestamp of when it was last heard.
    // Used internally by DiscoveryListener and exposed via KnownHosts.
    // -------------------------------------------------------------------------

    public class DiscoveredHost
    {
        public ServerBeacon Beacon { get; set; }
        public DateTime LastSeen { get; set; }
    }

}
