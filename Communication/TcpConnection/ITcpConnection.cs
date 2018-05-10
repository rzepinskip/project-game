using System;
using Common;

namespace Communication.TcpConnection
{
    public interface ITcpConnection
    {
        int Id { get; }
        ClientType ClientType { get; }

        void Receive();
        void Send(byte[] byteData);
        void SendKeepAlive();
        void CloseSocket();
        void FinalizeConnect(IAsyncResult ar);
        long GetLastMessageReceivedTicks();
        void UpdateLastMessageTicks();
    }
}