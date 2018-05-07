using System;
using Common;

namespace Communication
{
    public interface ITcpConnection
    {
        int SocketId { get; }
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