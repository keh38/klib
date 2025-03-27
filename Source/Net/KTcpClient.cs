using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


namespace KLib.Net
{
    public class KTcpClient
    {
        private TcpClient _socket = null;

        public int SendTimeout { get; set; }
        public int ReceiveTimeout { get; set; }
        public string LastError { get { return _lastError; } }
        public static bool ReverseBytes { set; get; } = false;

        private string _lastError;

        #region Static methods
        public static int SendMessage(IPEndPoint localEP, string message)
        {
            return SendMessage(localEP.Address.ToString(), localEP.Port, message);
        }
        public static async Task<int> SendMessageAsync(IPEndPoint localEP, string message)
        {
            return await Task.Run(() => SendMessage(localEP.Address.ToString(), localEP.Port, message));
        }
        public static async Task<int> SendMessageReceiveIntAsync(IPEndPoint localEP, string message)
        {
            return await Task.Run(() => SendMessageReceiveInt(localEP, message));
        }
        public static async Task<long> SendMessageReceiveLongAsync(IPEndPoint localEP, string message)
        {
            return await Task.Run(() => SendMessageReceiveLong(localEP, message));
        }
        public static async Task<byte[]> SendMessageReceiveByteArrayAsync(IPEndPoint localEP, string message)
        {
            return await Task.Run(() => SendMessageReceiveByteArray(localEP, message));
        }

        public static int SendMessage(string address, int port, string message)
        {
            int result = -1;

            try
            {
                var client = new KTcpClient();
                client.Connect(address, port);
                result = client.SendMessage(message);
                client.Close();
            }
            catch (Exception ex)
            {
            }

            return result;
        }

        public static int SendMessage(IPEndPoint localEP, string s, byte[] data)
        {
            int result = -1;

            try
            {
                var client = new KTcpClient();
                client.Connect(localEP.Address.ToString(), localEP.Port);
                result = client.SendMessage(s, data);
                client.Close();
            }
            catch (Exception ex)
            {
            }

            return result;
        }

        public static int SendMessage(IPEndPoint localEP, string s, string data)
        {
            int result = -1;

            try
            {
                var client = new KTcpClient();
                client.Connect(localEP.Address.ToString(), localEP.Port);
                result = client.SendMessage(s, data);
                client.Close();
            }
            catch (Exception ex)
            {
            }

            return result;
        }

        public static string SendMessageReceiveString(IPEndPoint localEP, string message)
        {
            string result = null;

            try
            {
                var client = new KTcpClient();
                client.Connect(localEP.Address.ToString(), localEP.Port);
                result = client.SendMessageReceiveString(message);
                client.Close();
            }
            catch (Exception ex)
            {
            }

            return result;
        }

        public static string SendMessageReceiveString(IPEndPoint localEP, string message, string data)
        {
            string result = null;

            try
            {
                var client = new KTcpClient();
                client.Connect(localEP.Address.ToString(), localEP.Port);
                result = client.SendMessageReceiveString(message, data);
                client.Close();
            }
            catch (Exception ex)
            {
            }

            return result;
        }

        public static int SendMessageReceiveInt(IPEndPoint localEP, string message)
        {
            int result = -1;

            try
            {
                var client = new KTcpClient();
                client.Connect(localEP.Address.ToString(), localEP.Port);
                result = client.SendMessageReceiveInt(message);
                client.Close();
            }
            catch (Exception ex)
            {
            }

            return result;
        }

        public static long SendMessageReceiveLong(IPEndPoint localEP, string message)
        {
            long result = 0;

            try
            {
                var client = new KTcpClient();
                client.Connect(localEP.Address.ToString(), localEP.Port);
                result = client.SendMessageReceiveLong(message);
                client.Close();
            }
            catch (Exception ex)
            {
            }

            return result;
        }

        public static byte[] SendMessageReceiveByteArray(IPEndPoint localEP, string message)
        {
            byte[] result = null;

            try
            {
                var client = new KTcpClient();
                client.Connect(localEP.Address.ToString(), localEP.Port);
                result = client.SendMessageReceiveByteArray(message);
                client.Close();
            }
            catch (Exception ex)
            {
            }

            return result;
        }

        public static byte[] SendMessageReceiveByteArray(IPEndPoint localEP, string message, byte[] data)
        {
            byte[] result = null;

            try
            {
                var client = new KTcpClient();
                client.Connect(localEP.Address.ToString(), localEP.Port);
                result = client.SendMessageReceiveByteArray(message, data);
                client.Close();
            }
            catch (Exception ex)
            {
            }

            return result;
        }

        #endregion


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

        public int SendMessage(string message)
        {
            int success = 0;

            using (NetworkStream theStream = _socket.GetStream())
            using (BinaryReader theReader = new BinaryReader(theStream))
            using (BinaryWriter theWriter = new BinaryWriter(theStream))
            {
                WriteStringAsByteArray(theWriter, message);

                success = ProcessInt32(theReader.ReadInt32());
            }

            return success;
        }

        public int SendMessage(string s, string data)
        {
            int result = -1;

            using (NetworkStream theStream = _socket.GetStream())
            using (BinaryReader theReader = new BinaryReader(theStream))
            using (BinaryWriter theWriter = new BinaryWriter(theStream))
            {
                WriteStringAsByteArray(theWriter, s);
                WriteStringAsByteArray(theWriter, data);

                result = ProcessInt32(theReader.ReadInt32());
            }

            return result;
        }

        public int SendMessage(string s, byte[] data)
        {
            int result = -1;

            using (NetworkStream theStream = _socket.GetStream())
            using (BinaryReader theReader = new BinaryReader(theStream))
            using (BinaryWriter theWriter = new BinaryWriter(theStream))
            {
                WriteStringAsByteArray(theWriter, s);

                theWriter.Write(ProcessInt32(data.Length));
                theWriter.Write(data);
                theWriter.Flush();

                result = ProcessInt32(theReader.ReadInt32());
            }

            return result;
        }

        public int SendMessageReceiveInt(string message)
        {
            int result = 0;

            using (NetworkStream theStream = _socket.GetStream())
            using (BinaryReader theReader = new BinaryReader(theStream))
            using (BinaryWriter theWriter = new BinaryWriter(theStream))
            {
                WriteStringAsByteArray(theWriter, message);
                result = theReader.ReadInt32();
            }

            return result;
        }

        public long SendMessageReceiveLong(string message)
        {
            long result = 0;

            using (NetworkStream theStream = _socket.GetStream())
            using (BinaryReader theReader = new BinaryReader(theStream))
            using (BinaryWriter theWriter = new BinaryWriter(theStream))
            {
                WriteStringAsByteArray(theWriter, message);
                result = theReader.ReadInt64();
            }

            return result;
        }

        public byte[] SendMessageReceiveByteArray(string message)
        {
            byte[] result = null;

            using (NetworkStream theStream = _socket.GetStream())
            using (BinaryReader theReader = new BinaryReader(theStream))
            using (BinaryWriter theWriter = new BinaryWriter(theStream))
            {
                WriteStringAsByteArray(theWriter, message);

                int count = ProcessInt32(theReader.ReadInt32());
                result = theReader.ReadBytes(count);
            }

            return result;
        }

        public byte[] SendMessageReceiveByteArray(string message, byte[] data)
        {
            byte[] result = null;

            using (NetworkStream theStream = _socket.GetStream())
            using (BinaryReader theReader = new BinaryReader(theStream))
            using (BinaryWriter theWriter = new BinaryWriter(theStream))
            {
                WriteStringAsByteArray(theWriter, message);

                theWriter.Write(data.Length);
                theWriter.Write(data);
                theWriter.Flush();

                int count = ProcessInt32(theReader.ReadInt32());
                if (count > 0)
                {
                    result = theReader.ReadBytes(count);
                }
            }

            return result;
        }

        public byte[] SendMessageReceiveByteArray(string message, string data)
        {
            byte[] result = null;

            using (NetworkStream theStream = _socket.GetStream())
            using (BinaryReader theReader = new BinaryReader(theStream))
            using (BinaryWriter theWriter = new BinaryWriter(theStream))
            {
                WriteStringAsByteArray(theWriter, message);
                WriteStringAsByteArray(theWriter, data);
                theWriter.Flush();

                int count = ProcessInt32(theReader.ReadInt32());
                if (count > 0)
                {
                    result = theReader.ReadBytes(count);
                }
            }

            return result;
        }

        public string SendMessageReceiveString(string message)
        {
            var bytes = SendMessageReceiveByteArray(message);
            return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        }

        public string SendMessageReceiveString(string message, string data)
        {
            var bytes = SendMessageReceiveByteArray(message, data);
            return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        }

        public void Close()
        {
            if (_socket != null)
            {
                _socket.Close();
                _socket = null;
            }
        }

        private void WriteStringAsByteArray(BinaryWriter theWriter, string message)
        {
            var byteArray = Encoding.UTF8.GetBytes(message);

            theWriter.Write(ProcessInt32(byteArray.Length));
            theWriter.Write(byteArray);
            theWriter.Flush();
        }

        private int ProcessInt32(int raw)
        {
            int value = raw;

            if (ReverseBytes)
            {
                var bytes = BitConverter.GetBytes(value);
                Array.Reverse(bytes);
                value = BitConverter.ToInt32(bytes, 0);
            }

            return value;
        }

    }
}