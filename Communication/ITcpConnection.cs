using System;

namespace Communication
{
    public interface ITcpConnection
    {
        int SocketId { get; }

        void Receive();
        void Send(byte[] byteData);
        void SendKeepAlive();
        void CloseSocket();
        void FinalizeConnect(IAsyncResult ar);
        long GetLastMessageReceivedTicks();
    }
}