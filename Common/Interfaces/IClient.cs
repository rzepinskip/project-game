using System;

namespace Common.Interfaces
{
    public interface IClient
    {
        void Connect(Action<Exception> connectionExceptionHandler, Action<IMessage> messageHandler);
        void Send(IMessage message);
    }
}