using System;

namespace Common.Interfaces
{
    public interface IClient
    {
        void Send(IMessage message);

        void SetupClient(Action<IMessage> messageHandler);
    }
}
