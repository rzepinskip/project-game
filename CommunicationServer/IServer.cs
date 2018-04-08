using System;

namespace CommunicationServer
{
    interface IServer
    {
        void Send(string data, int toId);
        void Send(string data, Guid fromGuid);
        void SetupServer(Action<string> messageHandler);
    }
}
