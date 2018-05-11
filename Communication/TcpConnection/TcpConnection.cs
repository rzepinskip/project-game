using System;
using System.Net.Sockets;
using System.Timers;
using Common;

namespace Communication.TcpConnection
{
    public abstract class TcpConnection : BaseTcpConnection
    {
        protected readonly double KeepAliveTimersInterval;
        protected readonly TimeSpan MaxUnresponsivenessDuration;
        protected Timer CheckReceivedKeepAlivesTimer;

        protected TcpConnection(int id, Socket socket, TimeSpan maxUnresponsivenessDuration,
            Action<CommunicationException> connectionFailureHandler, IMessageDeserializer messageDeserializer) : base(
            id, socket, connectionFailureHandler, messageDeserializer)
        {
            MaxUnresponsivenessDuration = maxUnresponsivenessDuration;
            KeepAliveTimersInterval = maxUnresponsivenessDuration.TotalMilliseconds /
                                      Constants.KeepAliveIntervalFrequencyDivisor;

            CheckReceivedKeepAlivesTimer = new Timer(KeepAliveTimersInterval);
            CheckReceivedKeepAlivesTimer.Elapsed += CheckReceivedKeepAlives;

            StartCheckReceivedKeepAlivesTimer();
        }

        public long GetLastReceivedMessageTicks()
        {
            return State.LastReceivedMessageTicks;
        }

        public void UpdateLastReceivedMessageTicks()
        {
            State.UpdateLastReceivedMessageTicks();
        }

        public void SendKeepAlive()
        {
            Send(new[] {Convert.ToByte(Constants.EtbByte)});
        }

        public void StartCheckReceivedKeepAlivesTimer()
        {
            CheckReceivedKeepAlivesTimer.Start();
        }

        public void StopCheckReceivedKeepAlivesTimer()
        {
            CheckReceivedKeepAlivesTimer.Stop();
        }

        private void CheckReceivedKeepAlives(object source, ElapsedEventArgs e)
        {
            var currentTime = DateTime.Now.Ticks;

            var elapsedTicks = currentTime - GetLastReceivedMessageTicks();
            var elapsedSpan = new TimeSpan(elapsedTicks);
            if (elapsedSpan > MaxUnresponsivenessDuration)
                throw new CommunicationException("Keep alive max interval exceeded", null,
                    CommunicationException.ErrorSeverity.Fatal);
        }


        public override void CloseConnection()
        {
            base.CloseConnection();

            CheckReceivedKeepAlivesTimer.Dispose();
        }
    }
}