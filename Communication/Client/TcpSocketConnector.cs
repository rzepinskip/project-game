using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Common.Interfaces;

namespace Communication.Client
{
    public class TcpSocketConnector : IConnector
    {
        private const int Port = 11000;

        private readonly ManualResetEvent _connectDone;
        private readonly IMessageDeserializer _messageDeserializer;
        private readonly Action<IMessage> _messageHandler;
        private readonly int _keepAliveInterval;
        public TcpSocketConnector(IMessageDeserializer messageDeserializer, Action<IMessage> messageHandler, int keepAliveInterval = 1000)
        {
            ConnectFinalized = new ManualResetEvent(false);
            _connectDone = new ManualResetEvent(false);
            _messageDeserializer = messageDeserializer;
            _messageHandler = messageHandler;
            _keepAliveInterval = keepAliveInterval;
        }

        public ITcpConnection TcpConnection { get; set; }
        public ManualResetEvent ConnectFinalized { get; set; }

        public void Connect()
        {
            try
            {
                var ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                var ipAddress = ipHostInfo.AddressList[0];
                var remoteEndPoint = new IPEndPoint(ipAddress, Port);

                var client = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                var tcpConnection = new ClientTcpConnection(client, -1, _messageDeserializer, _messageHandler);
                TcpConnection = tcpConnection;

                client.BeginConnect(remoteEndPoint, ConnectCallback, client);
                _connectDone.WaitOne();
                tcpConnection.StartKeepAliveTimer(_keepAliveInterval);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            StartReading();
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                TcpConnection.FinalizeConnect(ar);
                _connectDone.Set();
                ConnectFinalized.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void StartReading()
        {
            Debug.WriteLine("Client starts reading");
            while (true)
                TcpConnection.Receive();
        }
    }
}