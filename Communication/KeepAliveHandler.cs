using System;
using System.Collections.Generic;
using System.Threading;

namespace Communication
{
    public abstract class KeepAliveHandler
    {
        protected readonly TimeSpan KeepAliveTimeInterval;
        protected readonly IEnumerable<ITcpConnection> MaintainedConnections;
        protected Timer ReceivedKeepAlivesTimer;

        public KeepAliveHandler(TimeSpan keepAliveTimeInterval, IEnumerable<ITcpConnection> maintainedConnections)
        {
            MaintainedConnections = maintainedConnections;
            KeepAliveTimeInterval = keepAliveTimeInterval;
            ReceivedKeepAlivesTimer = new Timer(CheckKeepAlivesCallback, null, 25000,
                (keepAliveTimeInterval.Seconds*1000 + keepAliveTimeInterval.Milliseconds) / 8);
        }

        private void CheckKeepAlivesCallback(object state)
        {
            int counter = 0;
            var currentTime = DateTime.Now.Ticks;
            foreach (var csStateObject in MaintainedConnections)
            {
                var elapsedTicks = currentTime - csStateObject.GetLastMessageReceivedTicks();
                var elapsedSpan = new TimeSpan(elapsedTicks);
                Console.WriteLine(counter + " " + currentTime + " " + csStateObject.GetLastMessageReceivedTicks() + " " + elapsedSpan);
                if (elapsedSpan > 2*KeepAliveTimeInterval)
                    ConnectionFailureHandler(csStateObject);
                counter++;
            }
        }

        protected abstract void ConnectionFailureHandler(ITcpConnection connection);
    }
}