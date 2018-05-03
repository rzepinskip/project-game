using System.Collections;
using System.Collections.Generic;

namespace Communication.Client
{
    public class ClientMaintainedConnections : IEnumerable<ITcpConnection>
    {
        private readonly ITcpConnection _tcpConnection;

        public ClientMaintainedConnections(ITcpConnection tcpConnection)
        {
            _tcpConnection = tcpConnection;
        }

        public IEnumerator<ITcpConnection> GetEnumerator()
        {
            yield return _tcpConnection;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}