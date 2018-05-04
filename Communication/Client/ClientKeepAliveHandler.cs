using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Common;

namespace Communication.Client
{
    public class ClientKeepAliveHandler : KeepAliveHandler
    {
        private readonly Timer _sentKeepAliveTimer;
        private readonly Timer _helperTimer;
        public ClientKeepAliveHandler(TimeSpan keepAliveTimeInterval, IEnumerable<ITcpConnection> maintainedConnections)
            : base(keepAliveTimeInterval, maintainedConnections)
        {
            _helperTimer = new Timer((keepAliveTimeInterval.Seconds * 1000 + keepAliveTimeInterval.Milliseconds) / 8);
            _sentKeepAliveTimer = new Timer((keepAliveTimeInterval.Seconds * 1000 + keepAliveTimeInterval.Milliseconds) / 8);
            _sentKeepAliveTimer.Elapsed += SendKeepAliveCallback;
            _sentKeepAliveTimer.Start();
            _helperTimer.Elapsed += wakeUpMainTimer;
        }

        private void SendKeepAliveCallback(Object source, System.Timers.ElapsedEventArgs e)
        {
            MaintainedConnections.First().SendKeepAlive();
        }

        public void ResetTimer()
        {
            _helperTimer.Stop();
            _sentKeepAliveTimer.Stop();
            _helperTimer.Start();
            //_sentKeepAliveTimer.Change((KeepAliveTimeInterval.Seconds * 1000 + KeepAliveTimeInterval.Milliseconds) / 8, (KeepAliveTimeInterval.Seconds * 1000 + KeepAliveTimeInterval.Milliseconds) / 8);

        }

        private void wakeUpMainTimer(Object source, System.Timers.ElapsedEventArgs e)
        {
            _sentKeepAliveTimer.Start();
            _helperTimer.Stop();
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