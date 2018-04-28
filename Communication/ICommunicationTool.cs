using System;

namespace Communication
{
    public interface ICommunicationTool
    {
        void Receive();
        void Send(byte[] byteData);
        void CloseSocket();
        void EndConnectSocket(IAsyncResult ar);
    }
}