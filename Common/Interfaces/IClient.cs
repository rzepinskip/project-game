using System;

namespace Common.Interfaces
{
    public interface IClient
    {
        void Send(string data);

        void SetupClient(Action<string> messageHandler);
    }
}
