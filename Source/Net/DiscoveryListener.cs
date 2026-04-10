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
    // DiscoveryListener
    // Listens for UDP beacons and maintains a registry of known hosts.
    //
    // Events:
    //   HostDiscovered  — raised when a beacon is received from a new host.
    //   HostDisappeared — raised when a host has not beaconed within the
    //                     disappear threshold and is removed from KnownHosts.
    //
    // Usage:
    //   var listener = new DiscoveryListener();
    //   listener.HostDiscovered  += (s, beacon) => Console.WriteLine($"Found {beacon.Name}");
    //   listener.HostDisappeared += (s, beacon) => Console.WriteLine($"Lost {beacon.Name}");
    //   listener.Start();
    //   ...
    //   var hosts = listener.KnownHosts;  // query at any time
    //   ...
    //   listener.Stop();  // or listener.Dispose()
    // -------------------------------------------------------------------------

    public class DiscoveryListener : IDisposable
    {
        private readonly int _discoveryPort;
        private readonly int _disappearThresholdMs;
        private readonly int _watchdogIntervalMs;

        private UdpClient _udpClient;
        private Thread _listenerThread;
        private Timer _watchdogTimer;
        private bool _running;
        private bool _disposed;
        private readonly object _lock = new object();

        private readonly Dictionary<string, DiscoveredHost> _knownHosts
            = new Dictionary<string, DiscoveredHost>(StringComparer.OrdinalIgnoreCase);

        /// <summary>Raised on the listener thread when a new host is discovered.</summary>
        public event EventHandler<ServerBeacon> HostDiscovered;

        /// <summary>Raised on the watchdog timer thread when a host has stopped beaconing.</summary>
        public event EventHandler<ServerBeacon> HostDisappeared;

        /// <summary>
        /// Returns a snapshot of currently known hosts, keyed by server name.
        /// Safe to call from any thread.
        /// </summary>
        public IReadOnlyDictionary<string, DiscoveredHost> KnownHosts
        {
            get
            {
                lock (_knownHosts)
                {
                    return new Dictionary<string, DiscoveredHost>(_knownHosts,
                        StringComparer.OrdinalIgnoreCase);
                }
            }
        }

        /// <param name="discoveryPort">UDP port to listen on. Must match DiscoveryBeacon.</param>
        /// <param name="disappearThresholdSeconds">
        ///     How long a host can go without beaconing before it is considered gone.
        ///     Should be several multiples of the beacon interval.
        /// </param>
        /// <param name="watchdogIntervalSeconds">How often the watchdog checks for disappeared hosts.</param>
        public DiscoveryListener(
            int discoveryPort = 10001,
            int disappearThresholdSeconds = 10,
            int watchdogIntervalSeconds = 2)
        {
            _discoveryPort = discoveryPort;
            _disappearThresholdMs = disappearThresholdSeconds * 1000;
            _watchdogIntervalMs = watchdogIntervalSeconds * 1000;
        }

        /// <summary>Start listening for beacons and watching for disappeared hosts.</summary>
        public void Start()
        {
            lock (_lock)
            {
                if (_disposed)
                    throw new ObjectDisposedException(nameof(DiscoveryListener));
                if (_running)
                    return;

                // SO_REUSEADDR allows multiple processes on the same machine to
                // bind to the discovery port simultaneously (each will receive
                // its own copy of each incoming beacon).
                _udpClient = new UdpClient();
                _udpClient.Client.SetSocketOption(
                    SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                _udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, _discoveryPort));

                _running = true;

                _listenerThread = new Thread(ListenLoop)
                {
                    IsBackground = true,
                    Name = "DiscoveryListener"
                };
                _listenerThread.Start();

                _watchdogTimer = new Timer(
                    WatchdogTick, null, _watchdogIntervalMs, _watchdogIntervalMs);
            }
        }

        /// <summary>Stop listening and clear the known hosts registry.</summary>
        public void Stop()
        {
            lock (_lock)
            {
                if (!_running) return;
                _running = false;
                _watchdogTimer?.Dispose();
                _watchdogTimer = null;
                _udpClient?.Close(); // causes _udpClient.Receive() to throw, unblocking the thread
                _udpClient = null;
            }

            _listenerThread?.Join(2000);
            _listenerThread = null;
        }

        public void ForgetHost(string name)
        {
            // Remove the entry from whatever dictionary/set tracks known beacons
            if (_knownHosts.ContainsKey(name))
                _knownHosts.Remove(name);   // adjust to match actual field name
        }

        private void ListenLoop()
        {
            var endpoint = new IPEndPoint(IPAddress.Any, 0);

            while (_running)
            {
                try
                {
                    byte[] data = _udpClient.Receive(ref endpoint);
                    string json = Encoding.UTF8.GetString(data);
                    var beacon = JsonConvert.DeserializeObject<ServerBeacon>(json);

                    // Ignore malformed or unversioned packets.
                    if (beacon == null || string.IsNullOrWhiteSpace(beacon.Name))
                        continue;

                    bool isNew;
                    lock (_knownHosts)
                    {
                        isNew = !_knownHosts.ContainsKey(beacon.Name);
                        if (isNew)
                        {
                            _knownHosts[beacon.Name] = new DiscoveredHost
                            {
                                Beacon = beacon,
                                LastSeen = DateTime.UtcNow
                            };
                        }
                        else
                        {
                            // Refresh timestamp and update address/port in case they changed.
                            _knownHosts[beacon.Name].LastSeen = DateTime.UtcNow;
                            _knownHosts[beacon.Name].Beacon = beacon;
                        }
                    }

                    if (isNew)
                        HostDiscovered?.Invoke(this, beacon);
                }
                catch (SocketException)
                {
                    // Socket was closed by Stop() — exit cleanly.
                    break;
                }
                catch
                {
                    Thread.Sleep(10);
                    // Malformed packet or other transient error — keep going.
                }
            }
        }

        private void WatchdogTick(object state)
        {
            var now = DateTime.UtcNow;
            var disappeared = new List<ServerBeacon>();

            lock (_knownHosts)
            {
                var toRemove = new List<string>();
                foreach (var kvp in _knownHosts)
                {
                    if ((now - kvp.Value.LastSeen).TotalMilliseconds > _disappearThresholdMs)
                    {
                        toRemove.Add(kvp.Key);
                        disappeared.Add(kvp.Value.Beacon);
                    }
                }
                foreach (var key in toRemove)
                    _knownHosts.Remove(key);
            }

            // Fire events outside the lock to avoid deadlocks in event handlers.
            foreach (var beacon in disappeared)
                HostDisappeared?.Invoke(this, beacon);
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
