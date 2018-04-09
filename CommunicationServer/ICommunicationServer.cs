using System;
using Common.Interfaces;

namespace CommunicationServer
{
    interface ICommunicationServer
    {
        void Send(IMessage message, int id);
        void SetupServer(Action<IMessage, int> messageHandler);
    }
}
