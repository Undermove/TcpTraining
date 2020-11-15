using System;
using System.Net.Sockets;

namespace CleintClassNamespace
{
    public class ClientClass 
    {
        private string _serverAddress;

        public void Connect()
        {
            Int32 port = 13000;
            TcpClient client = new TcpClient(_serverAddress, port);
            NetworkStream stream = client.GetStream();
        }
    }
}