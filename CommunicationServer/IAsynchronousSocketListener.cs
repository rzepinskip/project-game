using System;
using Common;
using Common.Interfaces;

namespace CommunicationServer
{
    public interface IAsynchronousSocketListener
    {
        void Send(IMessage message, int connectionId);
        void StartListening(Action<CommunicationException> connectionExceptionHandler);
        void CloseConnection(int connectionId);
        bool IsConnectionExistent(int connectionId);
    }
}