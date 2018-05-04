using System.Collections.Generic;
using Common.Interfaces;
using Communication;

namespace CommunicationServer
{
    public class ClientManager : IClientManager
    {
        private readonly Dictionary<int, ITcpConnection> _agentToCommunicationHandler;
        public ClientManager(Dictionary<int, ITcpConnection> agentToCommunicationHandler)
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
