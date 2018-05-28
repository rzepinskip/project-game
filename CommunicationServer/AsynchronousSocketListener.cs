using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Common;
using Common.Interfaces;
using Communication;
using Communication.Errors;
using Communication.TcpConnection;

namespace CommunicationServer
{
    public class AsynchronousSocketListener : IAsynchronousSocketListener
    {
        private readonly TimeSpan _keepAliveTimeout;
        private readonly IMessageDeserializer _messageDeserializer;
        private readonly Action<IMessage, int> _messageHandler;
        private readonly int _port;

        private readonly ManualResetEvent _readyForAccept = new ManualResetEvent(false);
        private readonly Dictionary<int, ITcpConnection> _connectionIdToTcpConnection;
        private Action<CommunicationException> _connectionExceptionHandler;
        private int _nextConnectionId;
        private IPAddress _address;

        public AsynchronousSocketListener(int port, TimeSpan keepAliveTimeout,
            IMessageDeserializer messageDeserializer, Action<IMessage, int> messageHandler, IPAddress address)
        {
            _connectionIdToTcpConnection = new Dictionary<int, ITcpConnection>();
            _port = port;
            _nextConnectionId = 0;
            _messageHandler = messageHandler;
            _messageDeserializer = messageDeserializer;
            _keepAliveTimeout = keepAliveTimeout;
            _address = address;
        }

        public void Send(IMessage message, int connectionId)
        {
            var byteData = Encoding.ASCII.GetBytes(message.SerializeToXml() + Communication.Constants.EtbByte);
            Console.WriteLine(message.SerializeToXml());

            if (!IsConnectionExistent(connectionId))
                throw new IdentifiableCommunicationException(connectionId, $"Non existent connection during Send\n{message}", null, CommunicationException.ErrorSeverity.Temporary);

            var connection = _connectionIdToTcpConnection[connectionId];

            try
            {
                connection.Send(byteData);
            }
            catch (Exception e)
            {
                ConnectionError.PrintUnexpectedConnectionErrorDetails(e, connection.Id);
                throw;
            }
        }

        public void StartListening(Action<CommunicationException> connectionExceptionHandler)
        {
            var localEndPoint = new IPEndPoint(_address, _port);
            _connectionExceptionHandler = connectionExceptionHandler;

            var listeningSocket = new Socket(_address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                listeningSocket.Bind(localEndPoint);
                listeningSocket.Listen(100);

                while (true)
                {
                    _readyForAccept.Reset();
                    listeningSocket.BeginAccept(AcceptCallback, listeningSocket);

                    _readyForAccept.WaitOne();
                }
            }
            catch (Exception e)
            {
                ConnectionError.PrintUnexpectedConnectionErrorDetails(e);
                throw;
            }
        }

        public void CloseConnection(int connectionId)
        {
            if(!IsConnectionExistent(connectionId))
                throw new IdentifiableCommunicationException(connectionId, "Non existent connection during CloseConnection", null, CommunicationException.ErrorSeverity.Temporary);

            var tcpConnection = _connectionIdToTcpConnection[connectionId];

            tcpConnection.CloseConnection();
            _connectionIdToTcpConnection.Remove(connectionId);
        }

        public bool IsConnectionExistent(int connectionId)
        {
            return _connectionIdToTcpConnection.ContainsKey(connectionId);
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            Socket socket;
            var listener = ar.AsyncState as Socket;
            try
            {
                socket = listener.EndAccept(ar);
            }
            catch (Exception e)
            {
                ConnectionError.PrintUnexpectedConnectionErrorDetails(e, _nextConnectionId);
                throw;
            }            

            var state = new ServerTcpConnection(_nextConnectionId, socket, _connectionExceptionHandler, _keepAliveTimeout, _messageDeserializer,
                _messageHandler);
            _connectionIdToTcpConnection.Add(_nextConnectionId++, state);
            _readyForAccept.Set();
            StartReading(state);
        }

        private void StartReading(TcpConnection tool)
        {
            while (true)
                tool.Receive();
        }
    }
}