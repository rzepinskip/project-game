using System;

namespace Common.Interfaces
{
    public interface IAsynchronousSocketListener
    {
        void Send(IMessage message, int socketId);
        void StartListening(Action<Exception> connectionExceptionHandler);
        void CloseSocket(int socketId);
    }
}