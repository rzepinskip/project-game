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
            _sentKeepAliveTimer = new Timer(SendKeepAliveCallback, null, 20000, (keepAliveTimeInterval.Seconds*1000+keepAliveTimeInterval.Milliseconds) / 8);
        }

        private void SendKeepAliveCallback(object state)
        {
            MaintainedConnections.First().SendKeepAlive();
        }

        public void ResetTimer()
        {
            _sentKeepAliveTimer.Change((KeepAliveTimeInterval.Seconds * 1000 + KeepAliveTimeInterval.Milliseconds) / 8, (KeepAliveTimeInterval.Seconds * 1000 + KeepAliveTimeInterval.Milliseconds) / 8);
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