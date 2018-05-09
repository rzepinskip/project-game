using System;

namespace Common.Interfaces
{
    public interface ICommunicationClient
    {
        void Connect(Action<Exception> connectionExceptionHandler, Action<IMessage> messageHandler);
        void Send(IMessage message);
    }
}