using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Common;

namespace Communication.Client
{
    public class ClientKeepAliveHandler : KeepAliveHandler
    {
        private readonly Timer _helperTimer;
        private readonly Timer _sentKeepAliveTimer;

        public ClientKeepAliveHandler(TimeSpan keepAliveTimeInterval, IEnumerable<ITcpConnection> maintainedConnections)
            : base(keepAliveTimeInterval, maintainedConnections)
        {
            var keepAliveCheckInterval =
                keepAliveTimeInterval.TotalMilliseconds / Constants.KeepAliveIntervalFrequencyDivisor;
            _helperTimer = new Timer(keepAliveCheckInterval);
            _sentKeepAliveTimer = new Timer(keepAliveCheckInterval);
            _sentKeepAliveTimer.Elapsed += SendKeepAliveCallback;
            _sentKeepAliveTimer.Start();
            _helperTimer.Elapsed += WakeUpMainTimer;
        }

        private void SendKeepAliveCallback(object source, ElapsedEventArgs e)
        {
            MaintainedConnections.First().SendKeepAlive();
        }

        public void ResetTimer()
        {
            _sentKeepAliveTimer.Stop();
            _helperTimer.Start();
        }

        private void WakeUpMainTimer(object source, ElapsedEventArgs e)
        {
            _sentKeepAliveTimer.Start();
            _helperTimer.Stop();
        }

        protected override void ConnectionFailureHandler(ITcpConnection connection)
        {
            _sentKeepAliveTimer.Dispose();
            ReceivedKeepAlivesTimer.Dispose();
            connection.CloseSocket();

            throw new ApplicationFatalException("Keep alive timeout");
        }
    }
}