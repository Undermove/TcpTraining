using System.Net;
using System.Net.Sockets;

namespace ListenerNamespace
{
    public class Listener
    {
        public void Start()
        {
            TcpListener server = null;
            int port = 13000;
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");
            server = new TcpListener(localAddr, port);
            server.Start();
        }
    }
}