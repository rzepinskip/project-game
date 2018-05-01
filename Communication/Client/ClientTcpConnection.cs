using System;
using System.Net.Sockets;
using Common.Interfaces;

namespace Communication.Client
{
    public class ClientTcpConnection : TcpConnection
    {
        private readonly Action<IMessage> _handler;

        public ClientTcpConnection(Socket workSocket, int id, IMessageDeserializer messageDeserializer,
            Action<IMessage> handler, IKeepAliveGetter keepAliveGetter)
            : base(workSocket, id, messageDeserializer, keepAliveGetter)
        {
            _handler = handler;
        }

        public override void Handle(IMessage message, int id = -404)
        {
            _handler(message);
        }

        public override void HandleKeepAliveMessage()
        {
            
        }
    }
}