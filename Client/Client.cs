using System;
using System.Net.Sockets;

namespace ClientClassNamespace
{
    public class ClientClass 
    {
        private readonly string _serverAddress;
        private readonly int _port;

        public ClientClass(string serverAddress, int port)
        {
            _serverAddress = serverAddress;
            _port = port;
        }

        public void Connect()
        {
            TcpClient client = new TcpClient(_serverAddress, _port);
            NetworkStream stream = client.GetStream();
        }
    }
}