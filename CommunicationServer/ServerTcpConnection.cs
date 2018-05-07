using System;
using System.Net.Sockets;
using Common.Interfaces;
using Communication;
using Communication.Exceptions;

namespace CommunicationServer
{
    public class ServerTcpConnection : TcpConnection
    {
        private readonly Action<IMessage, int> _handleMessage;

        public ServerTcpConnection(Socket workSocket, int socketId, IMessageDeserializer messageDeserializer,
            Action<IMessage, int> handleMessage)
            : base(workSocket, socketId, messageDeserializer)
        {
            _handleMessage = handleMessage;
        }

        public override void Handle(IMessage message, int socketId = -404)
        {
            _handleMessage(message, socketId);
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