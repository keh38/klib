using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace KLib.Net
{

    // -------------------------------------------------------------------------
    // DiscoveryBeacon
    // Periodically broadcasts a UDP beacon advertising this server's presence.
    // The beacon is sent to the subnet broadcast address on the discovery port.
    //
    // Usage:
    //   var beacon = new DiscoveryBeacon("SERVER_A", "192.168.1.50", 9000);
    //   beacon.Start();
    //   ...
    //   beacon.Stop();  // or beacon.Dispose()
    // -------------------------------------------------------------------------

    public class DiscoveryBeacon : IDisposable
    {
        private readonly ServerBeacon _beacon;
        private readonly int _discoveryPort;
        private readonly int _intervalMs;

        private Timer _timer;
        private UdpClient _udpClient;
        private IPAddress _broadcastAddress;
        private bool _disposed;
        private readonly object _lock = new object();

        /// <param name="name">Unique server name.</param>
        /// <param name="tcpAddress">IP address of this server's TCP service.</param>
        /// <param name="tcpPort">Port of this server's TCP service.</param>
        /// <param name="discoveryPort">UDP port to broadcast on. Must match DiscoveryListener.</param>
        /// <param name="intervalSeconds">How often to broadcast the beacon.</param>
        public DiscoveryBeacon(
            string name,
            string tcpAddress,
            int tcpPort,
            int discoveryPort = 10001,
            int intervalSeconds = 2)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Server name must not be empty.", nameof(name));

            _beacon = new ServerBeacon
            {
                Name = name,
                Address = tcpAddress,
                TcpPort = tcpPort,
                Version = 1
            };

            _broadcastAddress = Discovery.GetDiscoveryAddress(multicast: false, address: IPAddress.Parse(tcpAddress)); 
            _discoveryPort = discoveryPort;
            _intervalMs = intervalSeconds * 1000;
        }

        /// <summary>Start broadcasting beacons.</summary>
        public void Start()
        {
            lock (_lock)
            {
                if (_disposed)
                    throw new ObjectDisposedException(nameof(DiscoveryBeacon));
                if (_timer != null)
                    return; // already running

                _udpClient = new UdpClient();
                _udpClient.EnableBroadcast = true;

                // Fire immediately, then repeat at every interval.
                _timer = new Timer(SendBeacon, null, 0, _intervalMs);
            }
        }

        /// <summary>Stop broadcasting beacons.</summary>
        public void Stop()
        {
            lock (_lock)
            {
                _timer?.Dispose();
                _timer = null;
                _udpClient?.Close();
                _udpClient = null;
            }
        }

        private void SendBeacon(object state)
        {
            UdpClient client;
            lock (_lock)
            {
                client = _udpClient;
            }

            if (client == null) return;

            try
            {
                string json = JsonConvert.SerializeObject(_beacon);
                byte[] data = Encoding.UTF8.GetBytes(json);
                client.Send(data, data.Length,
                    new IPEndPoint(_broadcastAddress, _discoveryPort));
            }
            catch
            {
                // Swallow — don't let a transient send failure crash the timer thread.
            }
        }

        public void Dispose()
        {
            lock (_lock)
            {
                if (!_disposed)
                {
                    Stop();
                    _disposed = true;
                }
            }
        }
    }


}
