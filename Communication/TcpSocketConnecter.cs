using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Common.Interfaces;

namespace Communication
{
    public class TcpSocketConnecter : IConnecter
    {
        private const int Port = 11000;

        private readonly ManualResetEvent _connectDone;
        private readonly IMessageConverter _messageConverter;
        private readonly Action<IMessage> _messageHandler;

        public TcpSocketConnecter(IMessageConverter messageConverter, Action<IMessage> messageHandler)
        {
            ConnectDoneForSend = new ManualResetEvent(false);
            _connectDone = new ManualResetEvent(false);
            _messageConverter = messageConverter;
            _messageHandler = messageHandler;
        }

        public ICommunicationTool ClientTcpCommunicationTool { get; set; }
        public ManualResetEvent ConnectDoneForSend { get; set; }

        public void Connect()
        {
            try
            {
                var ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                var ipAddress = ipHostInfo.AddressList[0];
                var remoteEp = new IPEndPoint(ipAddress, Port);

                var _client = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                ClientTcpCommunicationTool =
                    new ClientTcpCommunicationTool(_client, -1, _messageConverter, _messageHandler);

                _client.BeginConnect(remoteEp, ConnectCallback, _client);
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
                ClientTcpCommunicationTool.EndConnectSocket(ar);
                _connectDone.Set();
                ConnectDoneForSend.Set();
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
                ClientTcpCommunicationTool.Receive();
        }
    }
}