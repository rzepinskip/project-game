using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using Common.Interfaces;
using Messaging.Communication;

namespace CommunicationServer
{
    public class ServerCommunicationHandler : CommunicationHandler
    {
        private Action<IMessage, int> _handleMessage;
        public ServerCommunicationHandler(Socket workSocket, int id, IMessageConverter messageConverter, Action<IMessage, int> handleMessage)
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
