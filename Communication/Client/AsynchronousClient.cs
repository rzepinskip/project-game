using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Common.Interfaces;
using Communication.Exceptions;

namespace Communication.Client
{
    public class AsynchronousClient : IClient
    {
        private readonly ManualResetEvent _connectDone;
        private readonly ManualResetEvent _connectFinalized;
        private readonly IPEndPoint _ipEndPoint;
        private readonly TimeSpan _keepAliveInterval;
        private readonly IMessageDeserializer _messageDeserializer;

        private ITcpConnection _tcpConnection;

        public AsynchronousClient(IPEndPoint endPoint, TimeSpan keepAliveInterval, IMessageDeserializer messageDeserializer)
            
        {
            _connectFinalized = new ManualResetEvent(false);
            _connectDone = new ManualResetEvent(false);
            _messageDeserializer = messageDeserializer;
            _ipEndPoint = endPoint;
            _keepAliveInterval = keepAliveInterval == default(TimeSpan)
                ? Constants.DefaultMaxUnresponsivenessDuration
                : keepAliveInterval;
        }

        public void Send(IMessage message)
        {
            _connectFinalized.WaitOne();
            var byteData = Encoding.ASCII.GetBytes(message.SerializeToXml() + Constants.EtbByte);
            try
            {
                _tcpConnection.Send(byteData);
            }
            catch (Exception e)
            {
                throw new ConnectionException("Unable to send message", e);
            }
        }

        public void Connect(Action<Exception> connectionFailureHandler, Action<IMessage> messageHandler)
        {
            try
            {
                var client = new Socket(_ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                var tcpConnection = new ClientTcpConnection(client, -1, connectionFailureHandler, _messageDeserializer,
                    messageHandler);
                _tcpConnection = tcpConnection;

                client.BeginConnect(_ipEndPoint, ConnectCallback, client);
                _connectDone.WaitOne();
                tcpConnection.UpdateLastMessageTicks();
                tcpConnection.StartKeepAliveTimer(_keepAliveInterval);
            }
            catch (Exception e)
            {
                throw new ConnectionException("Unable to connect", e);
            }

            StartReading();
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                _tcpConnection.FinalizeConnect(ar);
                _connectDone.Set();
                _connectFinalized.Set();
            }
            catch (Exception e)
            {
                throw new ConnectionException("Unable to connect", e);
            }
        }

        private void StartReading()
        {
            while (true)
                try
                {
                    _tcpConnection.Receive();
                }
                catch (Exception e)
                {
                    throw new ConnectionException("Unable to read", e);
                }
        }
    }
}