using System.Collections.Generic;

namespace Communication.Client
{
    public class KeepAliveClientCollection : IKeepAliveGetter
    {
        private ITcpConnection _tcpConnection;

        public KeepAliveClientCollection(ITcpConnection tcpConnection)
        {
            _tcpConnection = tcpConnection;
        }

        public IEnumerable<ITcpConnection> Get()
        {
            yield return _tcpConnection;
        }
    }
}
