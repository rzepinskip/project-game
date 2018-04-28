using System;
using System.Net.Sockets;
using Common.Interfaces;

namespace Communication
{
    public class ClientTcpCommunicationTool : TcpCommunicationTool
    {
        private readonly Action<IMessage> _handler;

        public ClientTcpCommunicationTool(Socket workSocket, int id, IMessageConverter messageConverter,
            Action<IMessage> handler)
            : base(workSocket, id, messageConverter)
        {
            _handler = handler;
        }

        public override void Handle(IMessage message, int id = -404)
        {
            _handler(message);
        }
    }
}