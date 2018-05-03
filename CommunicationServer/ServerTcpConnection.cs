using System;
using System.Collections.Generic;
using System.Net.Sockets;
using Common.Interfaces;
using Communication;

namespace CommunicationServer
{
    public class ServerTcpConnection : TcpConnection
    {
        private readonly Action<IMessage, int> _handleMessage;

        public ServerTcpConnection(Socket workSocket, int id, IMessageDeserializer messageDeserializer,
            Action<IMessage, int> handleMessage, IEnumerable<ITcpConnection> maintainedConnections)
            : base(workSocket, id, messageDeserializer)
        {
            _handleMessage = handleMessage;
        }

        public override void Handle(IMessage message, int id = -404)
        {
            _handleMessage(message, id);
        }

        public override void HandleKeepAliveMessage()
        {
            SendKeepAlive();
        }
    }
}