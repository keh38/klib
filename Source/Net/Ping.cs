using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KLib.Net
{
    public class Pinger
    {
        private static List<Ping> pingers = new List<Ping>();
        private static int instances = 0;

        private static object @lock = new object();

        private static int result = 0;
        private static int timeOut = 250;

        private static int ttl = 5;

        private static List<string> _connections;

        public static bool PingAddress(string address)
        {
            PingOptions po = new PingOptions(ttl, true);
            ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            byte[] data = enc.GetBytes("abababababababababababababababab");

            var p = new Ping();
            var reply = p.Send(address);
            p.Dispose();

            return reply.Status == IPStatus.Success;
        }

        public static List<string> Sweep()
        {
            return SweepRange(1, 256);
        }

        public static void InitSweep()
        {
            CreatePingers(255 * 8);
        }

        public static void EndSweep()
        {
            DestroyPingers();
        }

        public static List<string> SweepRange(int lower, int upper)
        {
            string baseIP = "169.254.";

            Console.WriteLine($"Pinging destinations in {baseIP}{lower}.* - {baseIP}{upper-1}.*");

            _connections = new List<string>();

            //CreatePingers(255 * 8);

            PingOptions po = new PingOptions(ttl, true);
            ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            byte[] data = enc.GetBytes("abababababababababababababababab");

            int o3 = lower;
            int o4 = 1;

            Stopwatch watch = Stopwatch.StartNew();
            int numBlock = (upper-lower)/8;
            Console.WriteLine($"{numBlock} blocks");
            for (int kblock = 0; kblock < numBlock; kblock++)
            {
                //Console.WriteLine($"Pinging {baseIP}{o3} to {baseIP}{o3 + 7}");
                foreach (Ping p in pingers)
                {
                    if (o3 < upper)
                    {
                        lock (@lock)
                        {
                            instances += 1;
                        }

                        p.SendAsync($"{baseIP}{o3}.{o4}", timeOut, data, po);

                        o4++;
                        if (o4 == 256)
                        {
                            o3++;
                            o4 = 1;
                        }
                    }
                }

                while (instances > 0)
                {
                    Thread.Sleep(100);
                }
            }

            watch.Stop();

//            DestroyPingers();

            Console.WriteLine("Finished in {0}. Found {1} active IP-addresses.", watch.Elapsed.ToString(), result);

            return _connections;
        }

        private static void Ping_completed(object s, PingCompletedEventArgs e)
        {
            lock (@lock)
            {
                instances -= 1;
            }

            if (e.Reply.Status == IPStatus.Success)
            {
                Console.WriteLine(string.Concat("Active IP: ", e.Reply.Address.ToString()));
                result += 1;
                _connections.Add(e.Reply.Address.ToString());
            }
            else
            {
                //Console.WriteLine(String.Concat("Non-active IP: ", e.Reply.Address.ToString()));
            }
        }


        private static void CreatePingers(int cnt)
        {
            for (int i = 1; i <= cnt; i++)
            {
                Ping p = new Ping();
                p.PingCompleted += Ping_completed;
                pingers.Add(p);
            }
        }

        private static void DestroyPingers()
        {
            foreach (Ping p in pingers)
            {
                p.PingCompleted -= Ping_completed;
                p.Dispose();
            }

            pingers.Clear();

        }
    }
}
