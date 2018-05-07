using System;
using System.Collections.Generic;
using Communication;
using Communication.Exceptions;

namespace CommunicationServer
{
    public class ServerKeepAliveHandler : KeepAliveHandler
    {
        private readonly Action<Exception> _connectionTimeoutable;

        public ServerKeepAliveHandler(TimeSpan keepAliveTimeInterval, IEnumerable<ITcpConnection> maintainedConnections,
            Action<Exception> connectionTimeoutHandler)
            : base(keepAliveTimeInterval, maintainedConnections)
        {
            _connectionTimeoutable = connectionTimeoutHandler;
        }

        protected override void ConnectionFailureHandler(ITcpConnection connection)
        {
            throw new NotImplementedException();
            connection.CloseSocket();
        }
    }
}