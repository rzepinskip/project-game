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

        public ServerTcpConnection(Socket workSocket, int id, IMessageDeserializer messageDeserializer,
            Action<IMessage, int> handleMessage)
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

        protected override void HandleConnectionException(Exception e)
        {
            //Console.WriteLine("Next send will give us better exception so we do nothing");
        }
    }
}