using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Communication
{
    public interface ICommunicationTool
    {
        void Receive();
        void Send(byte[] byteData);
        void CloseSocket();
    }
}
