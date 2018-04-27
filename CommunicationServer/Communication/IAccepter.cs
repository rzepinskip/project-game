using System;
using System.Collections.Generic;
using System.Text;
using Common.Communication;

namespace CommunicationServer.Communication
{
    public interface IAccepter
    {
        Dictionary<int, TcpCommunicationTool> AgentToCommunicationHandler { get; set; }
        void StartListening();
    }
}
