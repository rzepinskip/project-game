using System;

namespace Communication
{
    public interface ITcpConnection
    {
        void Receive();
        void Send(byte[] byteData);
        void SendKeepAlive();
        void CloseSocket();
        void FinalizeConnect(IAsyncResult ar);
        long GetLastMessageReceivedTicks();
    }
}