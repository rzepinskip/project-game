using System;
using System.Net.Sockets;
using Common.Interfaces;
using Communication;

namespace CommunicationServer
{
    public class ServerTcpConnection : TcpConnection
    {
        private readonly Action<IMessage, int> _messageHandler;

        public ServerTcpConnection(Socket workSocket, int socketId, Action<Exception> connectionFailureHandler,
            IMessageDeserializer messageDeserializer,
            Action<IMessage, int> messageHandler)
            : base(workSocket, socketId, connectionFailureHandler, messageDeserializer)
        {
            _messageHandler = messageHandler;
        }

        public override void Handle(IMessage message, int socketId = -404)
        {
            _messageHandler(message, socketId);
        }

        public override void HandleKeepAliveMessage()
        {
            SendKeepAlive();
        }

        protected override void HandleConnectionException(Exception e)
        {
            //Console.WriteLine("Next send will give us better exception so we do nothing");
        }
    }
}