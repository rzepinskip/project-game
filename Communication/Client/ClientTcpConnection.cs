using System;
using System.Net.Sockets;
using Common.Interfaces;

namespace Communication.Client
{
    public class ClientTcpConnection : TcpConnection
    {
        private readonly Action<IMessage> _handler;
        private KeepAliveClientHandler _keepAliveClientHandler;

        public ClientTcpConnection(Socket workSocket, int id, IMessageDeserializer messageDeserializer,
            Action<IMessage> handler)
            : base(workSocket, id, messageDeserializer)
        {
            _handler = handler;
        }

        public void StartKeepAliveTimer(int keepAliveInterval)
        {
            _keepAliveClientHandler = new KeepAliveClientHandler(keepAliveInterval, new KeepAliveClientCollection(this));
        }

        public override void Handle(IMessage message, int id = -404)
        {
            _handler(message);
        }

        public override void Send(byte[] data)
        {
            _keepAliveClientHandler.ResetTimer();
            base.Send(data);
        }

        public override void HandleKeepAliveMessage()
        {
            
        }
    }
}