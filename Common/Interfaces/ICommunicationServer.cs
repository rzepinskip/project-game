using System;

namespace Common.Interfaces
{
    public interface ICommunicationServer
    {
        void Send(IMessage message, int id);
        void SetupServer(Action<IMessage, int> messageHandler);
    }
}
