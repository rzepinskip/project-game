using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Common.Communication
{
    public interface IConnecter
    {
        void Connect();
        ICommunicationTool ClientTcpCommunicationTool { get; set; }
        ManualResetEvent ConnectDoneForSend { get; set; }
    }
}
