using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Common;

namespace Communication.Client
{
    public class ClientKeepAliveHandler : KeepAliveHandler
    {
        private readonly Timer _sentKeepAliveTimer;

        public ClientKeepAliveHandler(TimeSpan keepAliveTimeInterval, IEnumerable<ITcpConnection> maintainedConnections)
            : base(keepAliveTimeInterval, maintainedConnections)
        {
            _sentKeepAliveTimer = new Timer(SendKeepAliveCallback, null, 0, keepAliveTimeInterval.Milliseconds / 2);
        }

        private void SendKeepAliveCallback(object state)
        {
            MaintainedConnections.First().SendKeepAlive();
        }

        public void ResetTimer()
        {
            _sentKeepAliveTimer.Change(0, KeepAliveTimeInterval.Milliseconds);
        }

        protected override void ConnectionFailureHandler(ITcpConnection connection)
        {
            _sentKeepAliveTimer.Dispose();
            ReceivedKeepAlivesTimer.Dispose();
            connection.CloseSocket();

            throw new GlobalException("Keep alive timeout");
        }
    }
}