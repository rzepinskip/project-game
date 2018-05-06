using System;

namespace Common.Interfaces
{
    public interface IClient
    {
        void SetIncomingMessageHandler(Action<IMessage> messageHandler);
        void Connect();
        void Send(IMessage message);
    }
}
