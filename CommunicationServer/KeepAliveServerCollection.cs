using System.Collections.Generic;
using Communication;

namespace CommunicationServer
{
    class KeepAliveServerCollection : IKeepAliveGetter
    {
        private readonly Dictionary<int, ITcpConnection> _agentToCommunicationHandler;

        public KeepAliveServerCollection(Dictionary<int, ITcpConnection> agentToCommunicationHandler)
        {
            _agentToCommunicationHandler = agentToCommunicationHandler;
        }

        public IEnumerable<ITcpConnection> Get()
        {
            return _agentToCommunicationHandler.Values;
        }
    }
}
