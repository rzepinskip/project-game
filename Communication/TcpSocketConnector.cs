using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Common.Interfaces;

namespace Communication
{
    public class TcpSocketConnector : IConnector
    {
        private const int Port = 11000;

        private readonly ManualResetEvent _connectDone;
        private readonly IMessageConverter _messageConverter;
        private readonly Action<IMessage> _messageHandler;

        public TcpSocketConnector(IMessageConverter messageConverter, Action<IMessage> messageHandler)
        {
            ConnectFinalized = new ManualResetEvent(false);
            _connectDone = new ManualResetEvent(false);
            _messageConverter = messageConverter;
            _messageHandler = messageHandler;
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
                TcpConnection =
                    new ClientTcpConnection(client, -1, _messageConverter, _messageHandler);

                client.BeginConnect(remoteEndPoint, ConnectCallback, client);
                _connectDone.WaitOne();
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