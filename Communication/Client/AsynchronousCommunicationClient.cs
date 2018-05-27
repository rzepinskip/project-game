using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Common;
using Common.Interfaces;
using Communication.Errors;
using Communication.TcpConnection;

namespace Communication.Client
{
    public class AsynchronousCommunicationClient : ICommunicationClient
    {
        private readonly ManualResetEvent _connectDone;
        private readonly ManualResetEvent _connectFinalized;
        private readonly IPEndPoint _ipEndPoint;
        private readonly TimeSpan _keepAliveTimeout;
        private readonly IMessageDeserializer _messageDeserializer;
        private bool _connectedToServer;
        private ITcpConnection _tcpConnection;

        public AsynchronousCommunicationClient(IPEndPoint endPoint, TimeSpan keepAliveTimeout,
            IMessageDeserializer messageDeserializer)

        {
            _connectFinalized = new ManualResetEvent(false);
            _connectDone = new ManualResetEvent(false);
            _messageDeserializer = messageDeserializer;
            _ipEndPoint = endPoint;
            _keepAliveTimeout = keepAliveTimeout == default(TimeSpan)
                ? Constants.DefaultMaxUnresponsivenessDuration
                : keepAliveTimeout;
            _connectedToServer = false;
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
                ConnectionError.PrintUnexpectedConnectionErrorDetails(e);
                throw;
            }
        }

        public void CloseConnection()
        {
            _tcpConnection.CloseConnection();
            _connectFinalized.Reset();
            _connectedToServer = false;
        }

        public void HandleCommunicationError(CommunicationException communicationException)
        {
            if (communicationException.Severity == CommunicationException.ErrorSeverity.Temporary)
            {
                Console.WriteLine("Temporary error during communication: " + communicationException.Message);
                return;
            }

            Console.WriteLine("Fatal error during communication: " + communicationException.Message);
            CloseConnection();
        }

        public void Connect(Action<CommunicationException> connectionFailureHandler, Action<IMessage> messageHandler)
        {
            while (!_connectedToServer)
            {
                _tcpConnection?.CloseConnection();
                try
                {
                    _connectDone.Reset();
                    var socket = new Socket(_ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    _tcpConnection = new ClientTcpConnection(-1, socket, connectionFailureHandler, _keepAliveTimeout,
                        _messageDeserializer, messageHandler);
                    socket.BeginConnect(_ipEndPoint, ConnectCallback, socket);
                    _connectDone.WaitOne();
                }
                catch (Exception e)
                {
                    ConnectionError.PrintUnexpectedConnectionErrorDetails(e);
                    throw;
                }
            }
            Console.WriteLine("Connected to CS");
            StartReading();
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                _tcpConnection.FinalizeConnect(ar);
                _connectedToServer = true;
                _connectDone.Set();
                _connectFinalized.Set();
            }
            catch (Exception e)
            {
                if (e is SocketException se && se.SocketErrorCode == SocketError.ConnectionRefused)
                {
                    _connectDone.Set();
                    return;
                }

                ConnectionError.PrintUnexpectedConnectionErrorDetails(e);
                throw;
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
                    ConnectionError.PrintUnexpectedConnectionErrorDetails(e);
                    throw;
                }
        }
    }
}