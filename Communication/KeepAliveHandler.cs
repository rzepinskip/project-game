using System;
using System.Collections.Generic;
using System.Threading;

namespace Communication
{
    public class KeepAliveHandler
    {
        private Timer _checkKeepAlivesTimer;
        protected readonly TimeSpan KeepAliveTimeInterval;
        protected readonly IEnumerable<ITcpConnection> MaintainedConnections;
        public KeepAliveHandler(TimeSpan keepAliveTimeInterval, IEnumerable<ITcpConnection> maintainedConnections)
        {
            MaintainedConnections = maintainedConnections;
            KeepAliveTimeInterval = keepAliveTimeInterval;
            //_checkKeepAlivesTimer = new Timer(CheckKeepAlivesCallback, null, 0,
            //    KeepAliveTimeInterval.Milliseconds/2);
        }

        private void CheckKeepAlivesCallback(object state)
        {
            var currentTime = DateTime.Now.Ticks;
            foreach (var csStateObject in MaintainedConnections)
            {
                var elapsedTicks = currentTime - csStateObject.GetLastMessageReceivedTicks();
                var elapsedSpan = new TimeSpan(elapsedTicks);

                if (elapsedSpan > KeepAliveTimeInterval)
                    ConnectionFailureHandler(csStateObject);
            }
        }

        protected virtual void ConnectionFailureHandler(ITcpConnection connection)
        {
            connection.CloseSocket();
        }
    }
}
