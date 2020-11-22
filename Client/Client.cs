using System.Threading;
using System;
using System.Net.Sockets;

namespace ClientClassNamespace
{
    public class ClientClass 
    {
        private readonly string _serverAddress;
        private readonly int _port;
        private NetworkStream _stream;
        private Thread _listeningThread;

        public ClientClass(string serverAddress, int port)
        {
            _serverAddress = serverAddress;
            _port = port;
        }

        public void Connect()
        {
            TcpClient client = new TcpClient(_serverAddress, _port);
            _stream = client.GetStream();
        }

        public void SendMessage(string message)
        {
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
            _stream.Write(data, 0, data.Length);
        }

        public event Action<string> OnMessageReceived;

        private void StartListening()
        {
            _listeningThread = new Thread(() => 
            {
                byte[] data = new byte[256];
                Int32 bytes = _stream.Read(data, 0, data.Length);
                string message = System.Text.Encoding.ASCII.GetString(data, 0, bytes);

                OnMessageReceived?.Invoke(message);
            });
        }
    }
}