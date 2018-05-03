using System.Collections;
using System.Collections.Generic;
using Communication;

namespace CommunicationServer
{
    public class ServerMaintainedConnections : IEnumerable<ITcpConnection>
    {
        private readonly Dictionary<int, ITcpConnection> _agentToCommunicationHandler;

        public ServerMaintainedConnections(Dictionary<int, ITcpConnection> agentToCommunicationHandler)
        {
            _agentToCommunicationHandler = agentToCommunicationHandler;
        }

        public IEnumerator<ITcpConnection> GetEnumerator()
        {
            return _agentToCommunicationHandler.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerable<ITcpConnection> Get()
        {
            return _agentToCommunicationHandler.Values;
        }
    }
}