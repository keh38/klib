using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLib.Net
{
    public class ARP
    {
        public static List<ARPTable> GetARPTables()
        {
            try
            {
                ARPTable currentTable = null;
                var arpTables = new List<ARPTable>();

                foreach (var line in GetARPResult().Split(new char[] { '\n', '\r' }))
                {
                    // Parse out all the MAC / IP Address combinations
                    if (!string.IsNullOrEmpty(line))
                    {
                        var pieces = (from piece in line.Split(new char[] { ' ', '\t' })
                                        where !string.IsNullOrEmpty(piece)
                                        select piece).ToArray();

                        if (line.StartsWith("Interface:"))
                        {
                            currentTable = new ARPTable(pieces[1]);
                            arpTables.Add(currentTable);
                        }
                        else if (pieces.Length == 3)
                        {
                            currentTable.ClientInfo.Add(new IPInfo(pieces[0], pieces[1], pieces[2]));
                        }
                    }
                }

                // Return list of IPInfo objects containing MAC / IP Address combinations
                return arpTables;
            }
            catch (Exception ex)
            {
                throw new Exception("ARP: Error Parsing 'arp -a' results", ex);
            }
        }

        /// <summary>
        /// This runs the "arp" utility in Windows to retrieve all the MAC / IP Address entries.
        /// </summary>
        /// <returns></returns>
        private static string GetARPResult()
        {
            Process p = null;
            string output = string.Empty;

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
            }
            catch (Exception ex)
            {
                throw new Exception("IPInfo: Error Retrieving 'arp -a' Results", ex);
            }
            finally
            {
                if (p != null)
                {
                    p.Close();
                }
            }

            return output;
        }
    }
}
