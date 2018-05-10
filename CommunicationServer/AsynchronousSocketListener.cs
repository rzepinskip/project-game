using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Common;
using Common.Interfaces;
using Communication;
using Communication.Exceptions;
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
        private readonly Dictionary<int, ITcpConnection> _socketIdToTcpConnection;
        private Action<CommunicationException> _connectionExceptionHandler;
        private int _counter;

        public AsynchronousSocketListener(int port, TimeSpan keepAliveTimeout,
            IMessageDeserializer messageDeserializer, Action<IMessage, int> messageHandler)
        {
            _socketIdToTcpConnection = new Dictionary<int, ITcpConnection>();
            _port = port;
            _counter = 0;
            _messageHandler = messageHandler;
            _messageDeserializer = messageDeserializer;
            _keepAliveTimeout = keepAliveTimeout;
        }

        public void Send(IMessage message, int socketId)
        {
            var byteData = Encoding.ASCII.GetBytes(message.SerializeToXml() + Constants.EtbByte);
            var findResult = _socketIdToTcpConnection.TryGetValue(socketId, out var handler);
            if (!findResult)
                throw new Exception("Non exsistent socket id");

            try
            {
                handler.Send(byteData);
            }
            catch (Exception e)
            {
                if (e is SocketException socketException &&
                    socketException.SocketErrorCode == SocketError.ConnectionReset) throw;

                ConnectionException.PrintUnexpectedConnectionErrorDetails(e);
                throw;
            }
        }

        public void StartListening(Action<CommunicationException> connectionExceptionHandler)
        {
            var ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            var ipAddress = ipHostInfo.AddressList[0];
            var localEndPoint = new IPEndPoint(ipAddress, _port);
            _connectionExceptionHandler = connectionExceptionHandler;

            var listeningSocket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
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
                ConnectionException.PrintUnexpectedConnectionErrorDetails(e);
                throw;
            }
        }

        public void CloseSocket(int socketId)
        {
            Console.WriteLine("Closing socket: " + socketId);
            var findResult = _socketIdToTcpConnection.TryGetValue(socketId, out var socket);
            if (!findResult)
                throw new Exception("Non existent socket id");

            socket.CloseSocket();
            _socketIdToTcpConnection.Remove(socketId);
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            _readyForAccept.Set();
            Socket socket;
            var listener = ar.AsyncState as Socket;
            try
            {
                socket = listener.EndAccept(ar);
            }
            catch (Exception e)
            {
                ConnectionException.PrintUnexpectedConnectionErrorDetails(e);
                throw;
            }            

            var state = new ServerTcpConnection(_counter, socket, _connectionExceptionHandler, _keepAliveTimeout, _messageDeserializer,
                _messageHandler);
            _socketIdToTcpConnection.Add(_counter++, state);
            StartReading(state);
        }

        private void StartReading(TcpConnection tool)
        {
            while (true)
                tool.Receive();
        }
    }
}