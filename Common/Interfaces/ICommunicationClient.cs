using System;

namespace Common.Interfaces
{
    public interface ICommunicationClient
    {
        void Connect(Action<CommunicationException> connectionExceptionHandler, Action<IMessage> messageHandler);
        void Send(IMessage message);
        void CloseConnection();
    }
}