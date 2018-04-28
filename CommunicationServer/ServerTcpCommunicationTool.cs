using System;
using System.Net.Sockets;
using Common.Interfaces;
using Communication;

namespace CommunicationServer
{
    public class ServerTcpCommunicationTool : TcpCommunicationTool
    {
        private readonly Action<IMessage, int> _handleMessage;

        public ServerTcpCommunicationTool(Socket workSocket, int id, IMessageConverter messageConverter,
            Action<IMessage, int> handleMessage)
            : base(workSocket, id, messageConverter)
        {
            _handleMessage = handleMessage;
        }

        public override void Handle(IMessage message, int id = -404)
        {
            _handleMessage(message, id);
        }
    }
}