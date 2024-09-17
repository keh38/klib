using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;


namespace KLib.Net
{
    public class KTcpClient
    {
        private TcpClient _socket = null;

        public int SendTimeout { get; set; }
        public int ReceiveTimeout { get; set; }
        public string LastError { get { return _lastError; } }

        private string _lastError;

        public void Connect(IPEndPoint localEP)
        {
            _socket = new TcpClient(localEP.Address.ToString(), localEP.Port);
        }

        public void Connect(string server)
        {
            var parts = server.Split(':');
            if (parts.Length != 2)
                throw new Exception("Invalid server specification: " + server);

            _socket = new TcpClient(parts[0], int.Parse(parts[1]));
        }

        public void Connect(string hostName, int port)
        {
            _socket = new TcpClient(hostName, port);
            _socket.SendTimeout = 1000;
            _socket.ReceiveTimeout = 1000;
        }

        public int WriteStringAsByteArray(string s)
        {
            int success = 0;

            var byteArray = System.Text.Encoding.UTF8.GetBytes(s);
            int nbytes = byteArray.Length;

            using (NetworkStream theStream = _socket.GetStream())
            using (BinaryReader theReader = new BinaryReader(theStream))
            using (BinaryWriter theWriter = new BinaryWriter(theStream))
            {
                theWriter.Write(nbytes);
                theWriter.Write(byteArray);
                theWriter.Flush();

                success = theReader.ReadInt32();
            }

            return success;
        }

        public int SendCommand(string s)
        {
            return WriteStringAsByteArray(s);
        }

        public static int SendCommand(IPEndPoint localEP, string s)
        {
            return SendCommand(localEP.Address.ToString(), localEP.Port, s);
        }

        public static int SendCommand(string address, int port, string s)
        {
            int result = -1;

            try
            {
                var client = new KTcpClient();
                client.Connect(address, port);
                result = client.SendCommand(s);
                client.Close();
            }
            catch (Exception ex)
            {
            }

            return result;
        }

        public int WriteStringsAsByteArrays(params string[] str)
        {
            int success = 0;

            using (NetworkStream theStream = _socket.GetStream())
            using (BinaryReader theReader = new BinaryReader(theStream))
            using (BinaryWriter theWriter = new BinaryWriter(theStream))
            {
                foreach (var s in str)
                {
                    var byteArray = System.Text.Encoding.UTF8.GetBytes(s);
                    int nbytes = byteArray.Length;

                    theWriter.Write(nbytes);
                    theWriter.Write(byteArray);
                    theWriter.Flush();

                    success = theReader.ReadInt32();
                }
            }

            return success;
        }

        public string WriteStringAndBytes(string s, string data)
        {
            string result = null;

            using (NetworkStream theStream = _socket.GetStream())
            using (BinaryReader theReader = new BinaryReader(theStream))
            using (BinaryWriter theWriter = new BinaryWriter(theStream))
            using (StreamWriter textWriter = new StreamWriter(theStream))
            {
                textWriter.WriteLine(s);
                textWriter.Flush();

                var byteArray = System.Text.Encoding.UTF8.GetBytes(data);
                int nbytes = byteArray.Length;

                theWriter.Write(nbytes);
                theWriter.Write(byteArray);
                theWriter.Flush();

                result = theReader.ReadString();
            }

            return result;
        }

        public string WriteStringToOutputStream(string s)
        {
            string result = "";

            //Console.WriteLine($"Sending {s}...");

            using (NetworkStream theStream = _socket.GetStream())
            using (StreamWriter theWriter = new StreamWriter(theStream))
            using (BinaryReader theReader = new BinaryReader(theStream))
            {
                theWriter.WriteLine(s);
                theWriter.Flush();
                result = theReader.ReadString();
            }

            return result;
        }

        public string ReadStringFromInputStream()
        {
            string result = null;

            using (NetworkStream theStream = _socket.GetStream())
            using (BinaryReader theReader = new BinaryReader(theStream))
            {
                result = theReader.ReadString();
            }

            return result;
        }

        public byte[] ReadByteArrayFromInputStream()
        {
            byte[] result = null;

            using (NetworkStream theStream = _socket.GetStream())
            using (BinaryReader theReader = new BinaryReader(theStream))
            {
            }

            return result;
        }

        public byte[] SendCommandReceiveByteArray(string s)
        {
            byte[] result = null;

            var byteArray = System.Text.Encoding.UTF8.GetBytes(s);
            int nbytes = byteArray.Length;

            using (NetworkStream theStream = _socket.GetStream())
            using (BinaryReader theReader = new BinaryReader(theStream))
            using (BinaryWriter theWriter = new BinaryWriter(theStream))
            {
                theWriter.Write(nbytes);
                theWriter.Write(byteArray);
                theWriter.Flush();

                int count = theReader.ReadInt32();
                result = theReader.ReadBytes(count);
            }

            return result;
        }

        public static int SendCommandAndByteArray(IPEndPoint localEP, string s, byte[] data)
        {
            int result = -1;

            try
            {
                var client = new KTcpClient();
                client.Connect(localEP.Address.ToString(), localEP.Port);
                result = client.SendCommandAndByteArray(s, data);
                client.Close();
            }
            catch (Exception ex)
            {
            }

            return result;

        }

        public int SendCommandAndByteArray(string s, byte[] data)
        {
            int result = -1;

            var byteArray = System.Text.Encoding.UTF8.GetBytes(s);
            int nbytes = byteArray.Length;

            using (NetworkStream theStream = _socket.GetStream())
            using (BinaryReader theReader = new BinaryReader(theStream))
            using (BinaryWriter theWriter = new BinaryWriter(theStream))
            {
                theWriter.Write(nbytes);
                theWriter.Write(byteArray);
                theWriter.Flush();

                theWriter.Write(data.Length);
                theWriter.Write(data);
                theWriter.Flush();

                result = theReader.ReadInt32();
            }

            return result;
        }

        public byte[] SendCommandAndByteArrayReceiveByteArray(string s, byte[] data)
        {
            byte[] result = null;

            var byteArray = System.Text.Encoding.UTF8.GetBytes(s);
            int nbytes = byteArray.Length;

            using (NetworkStream theStream = _socket.GetStream())
            using (BinaryReader theReader = new BinaryReader(theStream))
            using (BinaryWriter theWriter = new BinaryWriter(theStream))
            {
                theWriter.Write(nbytes);
                theWriter.Write(byteArray);
                theWriter.Flush();

                theWriter.Write(data.Length);
                theWriter.Write(data);
                theWriter.Flush();

                int count = theReader.ReadInt32();
                if (count > 0)
                {
                    result = theReader.ReadBytes(count);
                }
            }

            return result;
        }

        public void Close()
        {
            if (_socket != null)
            {
                _socket.Close();
                _socket = null;
            }
        }

        public int ReadIntFromInputStream()
        {
            int result = -1;

            using (NetworkStream theStream = _socket.GetStream())
            using (BinaryReader theReader = new BinaryReader(theStream))
            {
                result = theReader.ReadInt32();
            }

            return result;
        }


    }
}