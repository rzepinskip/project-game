using System.Collections.Generic;
using Communication;

namespace CommunicationServer.Accepters
{
    public interface IAccepter
    {
        Dictionary<int, TcpConnection> AgentToCommunicationHandler { get; set; }
        void StartListening();
    }
}