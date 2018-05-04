using System;
using System.Collections.Generic;
using System.Timers;


namespace Communication
{
    public abstract class KeepAliveHandler
    {
        protected readonly TimeSpan KeepAliveTimeInterval;
        protected readonly IEnumerable<ITcpConnection> MaintainedConnections;
        public Timer ReceivedKeepAlivesTimer;

        public KeepAliveHandler(TimeSpan keepAliveTimeInterval, IEnumerable<ITcpConnection> maintainedConnections)
        {
            MaintainedConnections = maintainedConnections;
            KeepAliveTimeInterval = keepAliveTimeInterval;
            ReceivedKeepAlivesTimer = new Timer((keepAliveTimeInterval.Seconds * 1000 + keepAliveTimeInterval.Milliseconds) / 8);
            ReceivedKeepAlivesTimer.Elapsed += CheckKeepAlivesCallback;
            ReceivedKeepAlivesTimer.Start();

                //new Timer(CheckKeepAlivesCallback, null, 25000,
                //(keepAliveTimeInterval.Seconds*1000 + keepAliveTimeInterval.Milliseconds) / 8);
        }

        private void CheckKeepAlivesCallback(Object source, System.Timers.ElapsedEventArgs e)
        {
            int counter = 0;
            var currentTime = DateTime.Now.Ticks;
            foreach (var csStateObject in MaintainedConnections)
            {
                var elapsedTicks = currentTime - csStateObject.GetLastMessageReceivedTicks();
                var elapsedSpan = new TimeSpan(elapsedTicks);
                Console.WriteLine(counter + " " + currentTime + " " + csStateObject.GetLastMessageReceivedTicks() + " " + elapsedSpan);
                if (elapsedSpan > KeepAliveTimeInterval)
                    ConnectionFailureHandler(csStateObject);
                counter++;
            }
        }

        protected abstract void ConnectionFailureHandler(ITcpConnection connection);
    }
}