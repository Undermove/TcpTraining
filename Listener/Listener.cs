using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Sockets;
using ClientClassNamespace;
using System.Threading;

namespace ListenerNamespace
{
    public class Listener
    {
        private bool _isListening;
        private TcpListener _server;
        private Dictionary<string, ClientClass> _connectedUsers = new Dictionary<string, ClientClass>();

        public void Start()
        {
            int port = 13000;
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");
            _server = new TcpListener(localAddr, port);
            _server.Start();
            StartWaitingForConnections();
        }

        private void StartWaitingForConnections()
        {
            _isListening = true;
            Thread thread = new Thread(() => {
                while (_isListening)
                {
                    TcpClient client = _server.AcceptTcpClient();
                    AddNewConnection(client);
                }
            });
            thread.Start();
        }

        private void AddNewConnection(TcpClient connection)
        {
            ClientClass user = new ClientClass(connection);
            user.Connect();
            user.OnMessageReceived += (message) => {
                // Base Auth
                if(message.Contains("Auth:")){
                    string userName = message.Split(':')[1];
                    _connectedUsers.Add(userName, user);
                    _connectedUsers.Remove(connection.Client.RemoteEndPoint.ToString());
                }

                if(message.Contains("MessageTo:")){
                    string toUser = message.Split(':')[1];
                    string userMessage = message.Split(':')[2];
                    _connectedUsers[toUser].SendMessage(userMessage);
                }
            };

            _connectedUsers.Add(connection.Client.RemoteEndPoint.ToString(), user);
        }

        private void StopWaitingForConnections()
        {
            _isListening = false;
        }

        public void Stop()
        {
            // StopWaitingForConnections()
            foreach (var connectedUser in _connectedUsers.Values)
            {
                connectedUser.Disconnect();
            }
            _server.Stop();
        }
    }
}