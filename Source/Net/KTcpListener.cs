using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Net.Sockets;


namespace KLib.Net
{
    public class KTcpListener
    {
        private TcpListener _listener = null;
        private TcpClient _client = null;

        private NetworkStream _network = null;
        private BinaryReader _theReader;
        private BinaryWriter _theWriter;

        private string _address;
        private int _port;

        private bool _reverseWriteBytes = false;

        public bool StartListener(int port)
        {
            return StartListener(port, true);
        }

        public bool StartListener(int port, bool reverseWriteBytes)
        {
            _reverseWriteBytes = reverseWriteBytes;

            _address = Discovery.FindServerAddress();
            _port = port;

            IPAddress localAddress;
            if (_address.Equals("localhost"))
            {
                localAddress = IPAddress.Loopback;
            }
            else
            {
                localAddress = IPAddress.Parse(_address);
            }
            _listener = new TcpListener(localAddress, port);
//            _listener = new TcpListener(port);
            _listener.Start();
            
            return true;
        }

        public string ListeningOn { get { return $"{_address}:{_port}"; } }

        public void CloseListener()
        {
            if (_listener != null)
            {
                _listener.Stop();
                _listener = null;
            }
        }

        public bool Pending()
        {
            return _listener.Pending();
        }

        public void AcceptTcpClient()
        {
            _client = _listener.AcceptTcpClient();
            _network = _client.GetStream();
            _theReader = new BinaryReader(_network);
            _theWriter = new BinaryWriter(_network);
        }

        public void CloseTcpClient()
        {
            _network.Dispose();
        }

        public void WriteLineToOutputStream(string s)
        {
            using (NetworkStream theStream = _client.GetStream())
            using (StreamWriter theWriter = new StreamWriter(theStream))
            {
                theWriter.WriteLine(s);
                theWriter.Flush();
            }
        }

        public void WriteStringAsByteArray(string s)
        {
            var byteArray = System.Text.Encoding.UTF8.GetBytes(s);
            int nbytes = byteArray.Length;

            if (_reverseWriteBytes)
            {
                var bytes = BitConverter.GetBytes(nbytes);
                Array.Reverse(bytes);
                nbytes = BitConverter.ToInt32(bytes, 0);
            }

            _theWriter.Write(nbytes);
            _theWriter.Write(byteArray);
            _theWriter.Flush();
        }

        public void WriteByteArray(byte[] byteArray)
        {
            int nbytes = byteArray.Length;

            if (_reverseWriteBytes)
            {
                var bytes = BitConverter.GetBytes(nbytes);
                Array.Reverse(bytes);
                nbytes = BitConverter.ToInt32(bytes, 0);
            }

            _theWriter.Write(nbytes);
            _theWriter.Write(byteArray);
            _theWriter.Flush();
        }

        public void WriteInt32ToOutputStream(int value)
        {
            _theWriter.Write(value);
            _theWriter.Flush();
        }

        public void WriteIntToOutputStream(int value)
        {
            using (NetworkStream theStream = _client.GetStream())
            using (StreamWriter theWriter = new StreamWriter(theStream))
            {
                theWriter.Write(value);
                theWriter.Flush();
            }
        }

        public string ReadString()
        {
            string result = null;

            int nbytes = _theReader.ReadInt32();
            var byteArray = _theReader.ReadBytes(nbytes);

            result = System.Text.Encoding.UTF8.GetString(byteArray, 0, byteArray.Length);

            _theWriter.Write((int)1);
            _theWriter.Flush();

            return result;
        }

        public byte[] ReadBytes()
        {
            byte[] result = null;

            int nbytes = _theReader.ReadInt32();
            result = _theReader.ReadBytes(nbytes);

            _theWriter.Write((int)1);
            _theWriter.Flush();

            return result;
        }

        public string ReadStringFromInputStream()
        {
            string result = null;

            int nbytes = _theReader.ReadInt32();
            if (_reverseWriteBytes)
            {
                var bytes = BitConverter.GetBytes(nbytes);
                Array.Reverse(bytes);
                nbytes = BitConverter.ToInt32(bytes, 0);
            }

            var byteArray = _theReader.ReadBytes(nbytes);
            result = System.Text.Encoding.UTF8.GetString(byteArray, 0, byteArray.Length);

            return result;
        }

        public byte[] ReadByteArrayFromInputStream()
        {
            byte[] result = null;

            int nbytes = _theReader.ReadInt32();
            if (_reverseWriteBytes)
            {
                var bytes = BitConverter.GetBytes(nbytes);
                Array.Reverse(bytes);
                nbytes = BitConverter.ToInt32(bytes, 0);
            }

            result = _theReader.ReadBytes(nbytes);

            return result;
        }

        public int ReadInt32FromInputStream()
        {
            int result = _theReader.ReadInt32();
            return result;
        }

        public void SendAcknowledgement()
        {
            _theWriter.Write((int)1);
            _theWriter.Flush();
        }
        public void SendAcknowledgement(bool success)
        {
            _theWriter.Write(success ? (int)1 : (int)-1);
            _theWriter.Flush();
        }

    }
}