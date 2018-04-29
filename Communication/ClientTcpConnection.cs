using System;
using System.Net.Sockets;
using Common.Interfaces;

namespace Communication
{
    public class ClientTcpConnection : TcpConnection
    {
        private readonly Action<IMessage> _handler;

        public ClientTcpConnection(Socket workSocket, int id, IMessageDeserializer messageDeserializer,
            Action<IMessage> handler)
            : base(workSocket, id, messageDeserializer)
        {
            _handler = handler;
        }

        public override void Handle(IMessage message, int id = -404)
        {
            _handler(message);
        }
    }
}