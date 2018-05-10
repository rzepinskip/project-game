using System;
using Common;

namespace Communication.TcpConnection
{
    public interface ITcpConnection
    {
        int Id { get; }
        ClientType ClientType { get; }

        void FinalizeConnect(IAsyncResult ar);
        void Send(byte[] byteData);
        void Receive();
        void CloseSocket();
    }
}