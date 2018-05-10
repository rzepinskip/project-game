using System;
using System.Net.Sockets;
using System.Timers;
using Common;
using Common.Interfaces;

namespace Communication.Client
{
    public class ClientTcpConnection : TcpConnection.TcpConnection
    {
        private readonly Action<IMessage> _messageHandler;
        protected Timer SendKeepAliveTimer;

        public ClientTcpConnection(int id, Socket socket, Action<CommunicationException> connectionFailureHandler,
            TimeSpan maxUnresponsivenessDuration, IMessageDeserializer messageDeserializer,
            Action<IMessage> messageHandler)
            : base(id, socket, maxUnresponsivenessDuration, connectionFailureHandler, messageDeserializer)
        {
            _messageHandler = messageHandler;
        }

        public override void Handle(IMessage message, int socketId = -404)
        {
            _messageHandler(message);
        }

        protected override void FinalizeSend(IAsyncResult ar)
        {
            ResetSendKeepAliveTimer();
            base.FinalizeSend(ar);
        }

        public override void FinalizeConnect(IAsyncResult ar)
        {
            base.FinalizeConnect(ar);
            UpdateLastMessageTicks();
            StartSendKeepAliveTimer();
        }

        private void StartSendKeepAliveTimer()
        {
            SendKeepAliveTimer = new Timer(KeepAliveTimersInterval);
            SendKeepAliveTimer.Elapsed += SendKeepAliveCallback;
            SendKeepAliveTimer.Start();
        }

        private void SendKeepAliveCallback(object source, ElapsedEventArgs e)
        {
            SendKeepAlive();
        }

        private void StopSendKeepAliveTimer()
        {
            SendKeepAliveTimer.Stop();
        }

        private void ResetSendKeepAliveTimer()
        {
            StopSendKeepAliveTimer();
            StartSendKeepAliveTimer();
        }

        protected override void HandleExpectedConnectionError(CommunicationException e)
        {
            StopSendKeepAliveTimer();
            e.Data.Add("socketId", Id);
            ConnectionFailureHandler(e);
        }
    }
}