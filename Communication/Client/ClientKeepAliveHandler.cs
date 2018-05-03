using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Communication.Client
{
    public class ClientKeepAliveHandler : KeepAliveHandler
    {
        private readonly Timer _sendKeepAliveTimer;
       
        public ClientKeepAliveHandler(TimeSpan keepAliveTimeInterval, IEnumerable<ITcpConnection> maintainedConnections) : base(keepAliveTimeInterval, maintainedConnections)
        {
            _sendKeepAliveTimer = new Timer(SendKeepAliveCallback, null, 0, keepAliveTimeInterval.Milliseconds/2);
        }

        private void SendKeepAliveCallback(object state)
        {
            MaintainedConnections.First().SendKeepAlive();
        }

        public void ResetTimer()
        {
            _sendKeepAliveTimer.Change(0, KeepAliveTimeInterval.Milliseconds);
        }

        protected override void ConnectionFailureHandler(ITcpConnection connection)
        {
            connection.CloseSocket();
        }
    }
}
