using System;
using System.Collections.Generic;
using Communication;

namespace CommunicationServer
{
    public class ServerKeepAliveHandler : KeepAliveHandler
    {
        private readonly IConnectionTimeoutable _connectionTimeoutable;

        public ServerKeepAliveHandler(TimeSpan keepAliveTimeInterval, IEnumerable<ITcpConnection> maintainedConnections,
            IConnectionTimeoutable connectionTimeoutable)
            : base(keepAliveTimeInterval, maintainedConnections)
        {
            _connectionTimeoutable = connectionTimeoutable;
        }

        protected override void ConnectionFailureHandler(ITcpConnection connection)
        {
            _connectionTimeoutable.HandleConnectionTimeout(connection.SocketId);
            connection.CloseSocket();
        }
    }
}