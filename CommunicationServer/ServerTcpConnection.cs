using System;
using System.Net.Sockets;
using Common;
using Common.Interfaces;
using Communication;
using Communication.TcpConnection;

namespace CommunicationServer
{
    public class ServerTcpConnection : TcpConnection
    {
        private readonly Action<IMessage, int> _messageHandler;

        public ServerTcpConnection(int id, Socket socket, Action<CommunicationException> connectionFailureHandler,
            TimeSpan maxUnresponsivenessDuration, IMessageDeserializer messageDeserializer,
            Action<IMessage, int> messageHandler)
            : base(id, socket, maxUnresponsivenessDuration, connectionFailureHandler, messageDeserializer)
        {
            _messageHandler = messageHandler;
        }

        public override void Handle(IMessage message, int socketId = -404)
        {
            _messageHandler(message, socketId);
        }

        protected override void FinalizeReceive(IAsyncResult ar)
        {
            base.FinalizeReceive(ar);
            SendKeepAlive();
        }
    }
}