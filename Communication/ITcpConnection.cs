using System;

namespace Communication
{
    public interface ITcpConnection
    {
        void Receive();
        void Send(byte[] byteData);
        void CloseSocket();
        void FinalizeConnect(IAsyncResult ar);
    }
}