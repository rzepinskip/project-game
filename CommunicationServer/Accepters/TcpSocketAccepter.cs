using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Common.Interfaces;
using Communication;
using Communication.Exceptions;

namespace CommunicationServer.Accepters
{
    public class TcpSocketAccepter : IAccepter
    {
        private readonly IMessageDeserializer _messageDeserializer;
        private readonly Action<IMessage, int> _messageHandler;

        private readonly ManualResetEvent _readyForAccept = new ManualResetEvent(false);
        private int _counter;
        private KeepAliveHandler _keepAliveHandler;
        public TcpSocketAccepter(Action<IMessage, int> messageHandler, IMessageDeserializer messageDeserializer, TimeSpan keepAliveInterval, IConnectionTimeoutable connectionTimeoutHandler)
        {
            AgentToCommunicationHandler = new Dictionary<int, ITcpConnection>();
            _counter = 0;
            _messageHandler = messageHandler;
            _messageDeserializer = messageDeserializer;
            _keepAliveHandler = new ServerKeepAliveHandler(keepAliveInterval,
                new ServerMaintainedConnections(AgentToCommunicationHandler), connectionTimeoutHandler);
        }

        public Dictionary<int, ITcpConnection> AgentToCommunicationHandler { get; set; }

        public void StartListening()
        {
            var ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            var ipAddress = ipHostInfo.AddressList[0];
            var localEndPoint = new IPEndPoint(ipAddress, 11000);

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
            AgentToCommunicationHandler.Add(_counter++, state);
            StartReading(state);
        }

        private void StartReading(TcpConnection tool)
        {
            while (true)
                tool.Receive();
        }
    }
}