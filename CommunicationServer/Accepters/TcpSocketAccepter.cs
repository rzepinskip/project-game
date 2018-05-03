using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Common.Interfaces;
using Communication;

namespace CommunicationServer.Accepters
{
    public class TcpSocketAccepter : IAccepter
    {
        private readonly IMessageDeserializer _messageDeserializer;
        private readonly Action<IMessage, int> _messageHandler;

        private readonly ManualResetEvent _readyForAccept = new ManualResetEvent(false);
        private int _counter;

        public TcpSocketAccepter(Action<IMessage, int> messageHandler, IMessageDeserializer messageDeserializer)
        {
            AgentToCommunicationHandler = new Dictionary<int, TcpConnection>();
            _counter = 0;
            _messageHandler = messageHandler;
            _messageDeserializer = messageDeserializer;
        }

        public Dictionary<int, TcpConnection> AgentToCommunicationHandler { get; set; }

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
                    Debug.WriteLine("Waiting for a connection...");
                    listener.BeginAccept(AcceptCallback, listener);

                    _readyForAccept.WaitOne();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            _readyForAccept.Set();
            var handler = default(Socket);
            var listener = ar.AsyncState as Socket;
            try
            {
                handler = listener.EndAccept(ar);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Debug.WriteLine("Accepted for " + _counter);
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