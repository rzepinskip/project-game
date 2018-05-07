using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Common.Interfaces;
using Communication;
using Communication.Exceptions;

namespace CommunicationServer
{
    public class AsynchronousSocketListener : IAsynchronousSocketListener
    {
        private readonly Dictionary<int, ITcpConnection> _agentToCommunicationHandler;
        private readonly IMessageDeserializer _messageDeserializer;
        private readonly Action<IMessage, int> _messageHandler;

        private readonly int _port;

        private readonly ManualResetEvent _readyForAccept = new ManualResetEvent(false);
        private int _counter;
        private KeepAliveHandler _keepAliveHandler;

        public AsynchronousSocketListener(Action<IMessage, int> messageHandler,
            IMessageDeserializer messageDeserializer,
            TimeSpan keepAliveInterval, IConnectionTimeoutable connectionTimeoutHandler, int port)
        {
            _agentToCommunicationHandler = new Dictionary<int, ITcpConnection>();
            _port = port;
            _counter = 0;
            _messageHandler = messageHandler;
            _messageDeserializer = messageDeserializer;
            _keepAliveHandler = new ServerKeepAliveHandler(keepAliveInterval,
                new ServerMaintainedConnections(_agentToCommunicationHandler), connectionTimeoutHandler);
        }

        public void Send(IMessage message, int id)
        {
            var byteData = Encoding.ASCII.GetBytes(message.SerializeToXml() + Constants.EtbByte);
            var findResult = _agentToCommunicationHandler.TryGetValue(id, out var handler);
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

        public void StartListening()
        {
            var ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            var ipAddress = ipHostInfo.AddressList[0];
            var localEndPoint = new IPEndPoint(ipAddress, _port);

            var listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                while (true)
                {
                    _readyForAccept.Reset();
                    Debug.WriteLine("CS waiting for a connections...");
                    listener.BeginAccept(AcceptCallback, listener);

                    _readyForAccept.WaitOne();
                }
            }
            catch (Exception e)
            {
                throw new ConnectionException("Unable to start listening", e);
            }
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            _readyForAccept.Set();
            Socket handler;
            var listener = ar.AsyncState as Socket;
            try
            {
                handler = listener.EndAccept(ar);
            }
            catch (Exception e)
            {
                throw new ConnectionException("Unable to start listening", e);
            }

            Debug.WriteLine("CS accepted connection for " + _counter);
            var state = new ServerTcpConnection(handler, _counter, _messageDeserializer, _messageHandler);
            _agentToCommunicationHandler.Add(_counter++, state);
            StartReading(state);
        }

        private void StartReading(TcpConnection tool)
        {
            while (true)
                tool.Receive();
        }
    }
}