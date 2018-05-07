using System.Collections.Generic;
using Communication;

namespace CommunicationServer.Accepters
{
    public interface IAccepter
    {
        Dictionary<int, ITcpConnection> AgentToCommunicationHandler { get; set; }
        void StartListening();
    }
}