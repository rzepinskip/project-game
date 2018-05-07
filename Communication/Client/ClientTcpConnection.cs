using System;
using System.Net.Sockets;
using Common;
using Common.Interfaces;

namespace Communication.Client
{
    public class ClientTcpConnection : TcpConnection
    {
        private readonly Action<IMessage> _messageHandler;

        private ClientKeepAliveHandler _clientKeepAliveHandler;

        public ClientTcpConnection(Socket workSocket, int socketId, Action<Exception> connectionFailureHandler,
            IMessageDeserializer messageDeserializer, Action<IMessage> messageHandler)
            : base(workSocket, socketId, connectionFailureHandler, messageDeserializer)
        {
            _messageHandler = messageHandler;
        }

        public void StartKeepAliveTimer(TimeSpan keepAliveInterval)
        {
            _clientKeepAliveHandler =
                new ClientKeepAliveHandler(keepAliveInterval, new ClientMaintainedConnections(this));
        }

        public override void Handle(IMessage message, int socketId = -404)
        {
            _messageHandler(message);
        }

        public override void Send(byte[] data)
        {
            _clientKeepAliveHandler.ResetTimer();
            base.Send(data);
        }

        public override void HandleKeepAliveMessage()
        {
        }
    }
}