using System;

namespace Common.Interfaces
{
    public interface IProxy
    {
        //IClient Client { get; set; }

        void Send(IMessage message);
        void SubscribeProxy(Action<IMessage> messageHandler);

    }
}
