using System;

namespace Common.Interfaces
{
    public interface IServer
    {
        void Send(IMessage message, int id);
        void StartListening();
    }
}