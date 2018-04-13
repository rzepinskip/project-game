using System;
using System.Net.Sockets;
using Common.Interfaces;

namespace CommunicationServer
{
    class ServerProxy : IProxy
    {
        private ICommunicationServer Server { get; }
        public event Action<IMessage> MessageReceivedEvent;

        public ServerProxy(ICommunicationServer server)
        {
            Server = server;
            //Server.SetupServer(MessageHandler);
        }
        public void Send(IMessage message)
        {
            string content = "";
            //Socket socket = new Socket();
            //Server.Send(content, socket);
        }

        public void SubscribeProxy(Action<IMessage> messageHandler)
        {
            MessageReceivedEvent += messageHandler;
        }

        public void MessageHandler(string message)
        {
            //do sth
        }
    }
}
