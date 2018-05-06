using System.Collections.Generic;
using Common.Interfaces;
using Communication;

namespace CommunicationServer
{
    public class ClientTypeManager : IClientTypeManager
    {
        private readonly Dictionary<int, ITcpConnection> _agentToCommunicationHandler;

        public ClientTypeManager(Dictionary<int, ITcpConnection> agentToCommunicationHandler)
        {
            _agentToCommunicationHandler = agentToCommunicationHandler;
        }

        public void MarkClientAsPlayer(int id)
        {
            _agentToCommunicationHandler[id].MarkClientAsPlayer();
        }

        public void MarkClientAsGameMaster(int id)
        {
            _agentToCommunicationHandler[id].MarkClientAsGameMaster();
        }
    }
}