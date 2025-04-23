using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace KLib.Net
{
    public static class Discovery
    {
        public static string ByteFormat { get; private set; }

        public static IPEndPoint FindNextAvailableEndPoint()
        {
            int numPortsToTry = 1000;
            int port = 4950;

            string address = FindServerAddress();

            IPAddress ipAddress = null;
            if (address.Equals("localhost"))
            {
                ipAddress = IPAddress.Loopback;
            }
            else
            {
                ipAddress = IPAddress.Parse(address);
            }

            TcpListener listener = null; ;
            bool success = false;
            for (int k=0; k < numPortsToTry; k++)
            {
                try
                {
                    listener = new TcpListener(ipAddress, port);
                    listener.Start();
                    success = true;
                }
                catch (Exception ex)
                {
                    port++;
                }

                if (success)
                {
                    listener?.Stop();
                    break;
                }
            }

            if (success)
            {
                return new IPEndPoint(ipAddress, port);
            }

            return null;
        }

        public static string FindServerAddress()
        {
            return FindServerAddress(true);
        }

        /// <summary>
        /// Finds IP address belonging to a LAN on which to run a TCP server.
        /// </summary>
        /// <remarks>
        /// Parses ARP table to find a valid LAN address (starting with 169.254 or 11.12.13). Optionally defaults to localhost if no NIC found.
        /// </remarks>
        /// <param name="canUseLocalhost">specifies whether a localhost connection is allowed. Defaults to true.</param>
        /// <returns>IP address of NIC attached to LAN</returns>
        public static string FindServerAddress(bool canUseLocalhost)
        {
            Process p = null;
            string output = string.Empty;
            string address = string.Empty;

            try
            {
                p = Process.Start(new ProcessStartInfo("arp", "-a")
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true
                });

                output = p.StandardOutput.ReadToEnd();
                p.Close();

                foreach (var line in output.Split(new char[] { '\n', '\r' }))
                {
                    // Parse out all the MAC / IP Address combinations
                    if (!string.IsNullOrEmpty(line))
                    {
                        var pieces = (from piece in line.Split(new char[] { ' ', '\t' })
                                      where !string.IsNullOrEmpty(piece)
                                      select piece).ToArray();

                        // auto-configured LAN
                        if (line.StartsWith("Interface:") && pieces[1].StartsWith("192.168"))
                        {
                            address = pieces[1];
                            return address;
                        }
                        // auto-configured direct connection
                        if (line.StartsWith("Interface:") && pieces[1].StartsWith("169.254"))
                        {
                            address = pieces[1];
                            return address;
                        }
                        // LAN configured using 11.12.13.xxx convention
                        if (line.StartsWith("Interface:") && pieces[1].StartsWith("11.12.13"))
                        {
                            address = pieces[1];
                            return address;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving 'arp -a' results", ex);
            }
            finally
            {
                if (p != null)
                {
                    p.Close();
                }
            }

            if (string.IsNullOrEmpty(address) && canUseLocalhost)
            {
                address = "localhost";
            }

            return address;
        }
        public static IPEndPoint Discover(string name, int timeOut)
        {
            return Discover(name, timeOut, "");
        }
        /// <summary>
        /// Send UDP multicast message to find discoverable TCP server address
        /// </summary>
        /// <param name="name">Name of desired TCP server (typically all caps)</param>
        /// <param name="address"></param>
        /// <returns>Returns IPEndPoint object</returns>
        public static IPEndPoint Discover(string name, int timeOut = 500, string server="")
        {
            UdpClient udp = null;
            IPEndPoint endPoint = null;

            try
            {
                var addy = string.IsNullOrEmpty(server) ? FindServerAddress() : server;
                //Debug.WriteLine("discovering on: " + addy);

                IPAddress localAddress;
                if (addy.Equals("localhost"))
                {
                    localAddress = IPAddress.Loopback;
                }
                else
                {
                    localAddress = IPAddress.Parse(addy);
                }

                var ipLocal = new IPEndPoint(localAddress, 5555 + name.Length);
                Debug.WriteLine($"discovering {name} on: " + ipLocal);

                var address = IPAddress.Parse("234.5.6.7");
                var ipEndPoint = new IPEndPoint(address, 10000);

                udp = new UdpClient(ipLocal);
                udp.Client.ReceiveTimeout = timeOut;

                udp.JoinMulticastGroup(address, localAddress);
                udp.Send(Encoding.UTF8.GetBytes(name), name.Length, ipEndPoint);

                var anyIP = new IPEndPoint(IPAddress.Any, 0);

                var bytes = udp.Receive(ref anyIP);
                var response = Encoding.Default.GetString(bytes);
                var responseParts = response.Split(';');

                var port = Int32.Parse(responseParts[0]);
                endPoint = new IPEndPoint(anyIP.Address, port);

                //Debug.WriteLine("host = " + endPoint);

                ByteFormat = (responseParts.Length > 1) ? responseParts[1] : "";
            }
            catch (Exception ex)
            {
                //Debug.WriteLine($"Discover error, {name}: {ex.Message}");
            }

            if (udp != null)
            {
                udp.Close();
            }

            return endPoint;
        }

    }
}
