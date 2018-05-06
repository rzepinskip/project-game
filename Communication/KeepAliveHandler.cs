using System;
using System.Collections.Generic;
using System.Timers;

namespace Communication
{
    public abstract class KeepAliveHandler
    {
        protected readonly TimeSpan KeepAliveTimeInterval;
        protected readonly IEnumerable<ITcpConnection> MaintainedConnections;
        protected Timer ReceivedKeepAlivesTimer;

        protected KeepAliveHandler(TimeSpan keepAliveTimeInterval, IEnumerable<ITcpConnection> maintainedConnections)
        {
            MaintainedConnections = maintainedConnections;
            KeepAliveTimeInterval = keepAliveTimeInterval;
            ReceivedKeepAlivesTimer =
                new Timer(keepAliveTimeInterval.TotalMilliseconds / Constants.KeepAliveIntervalFrequencyDivisor);
            ReceivedKeepAlivesTimer.Elapsed += CheckKeepAlivesCallback;
            ReceivedKeepAlivesTimer.Start();
        }

        private void CheckKeepAlivesCallback(object source, ElapsedEventArgs e)
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

        protected abstract void ConnectionFailureHandler(ITcpConnection connection);
    }
}