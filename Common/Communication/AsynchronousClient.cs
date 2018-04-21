using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Common.Interfaces;

namespace Common.Communication
{

    public class AsynchronousClient : IClient
    {
        private const int Port = 11000;
        private readonly ManualResetEvent _connectDone =
            new ManualResetEvent(false);
        private readonly ManualResetEvent _connectDoneForSend =
            new ManualResetEvent(false);
        private readonly ManualResetEvent _receiveDone =
            new ManualResetEvent(false);

        public event Action<IMessage> MessageReceivedEvent;
        private readonly IMessageConverter _messageConverter;

        private Socket _client;

        public AsynchronousClient(IMessageConverter messageConverter)
        {
            _messageConverter = messageConverter;
        }

        public void StartClient()
        {
            try
            {
                var ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                var ipAddress = ipHostInfo.AddressList[0];
                var remoteEp = new IPEndPoint(ipAddress, Port);

                _client = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                _client.BeginConnect(remoteEp, ConnectCallback, _client);
                _connectDone.WaitOne();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            //var thread = new Thread(StartReading);
            //thread.Start();
            StartReading();

        }

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {

                _client.EndConnect(ar);

                Console.WriteLine("Socket connected to {0}", _client.RemoteEndPoint);

                _connectDone.Set();
                _connectDoneForSend.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void StartReading()
        {
            var state = new CommunicationStateObject(_client);
            Console.WriteLine("Client starts reading");
            while (true)
            {
                _receiveDone.Reset();

                Receive(_client, state);

                _receiveDone.WaitOne();
            }
        }

        public void Receive(Socket socket, CommunicationStateObject state)
        {
 
            //according to docs beginreceive can throw an exception
            try
            {
                _client.BeginReceive(state.Buffer, 0, CommunicationStateObject.BufferSize, 0, ReceiveCallback, state);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            var response = string.Empty;
            try
            {
                var state = (CommunicationStateObject)ar.AsyncState;
                var client = state.WorkSocket;
                var bytesRead = 0;

                try
                {
                    bytesRead = client.EndReceive(ar);
                    Debug.WriteLine("received bytes" + bytesRead);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    return;
                }
                
                if (bytesRead > 0)
                {
                    state.Sb.Append(Encoding.ASCII.GetString(state.Buffer, 0, bytesRead));
                    response = state.Sb.ToString();
                    Debug.WriteLine(response);
                    if (response.IndexOf(CommunicationStateObject.EtbByte) > -1)
                    {
                        var messages = response.Split(CommunicationStateObject.EtbByte);
                        var numberOfMessages = messages.Length;
                        var wholeMessages = string.IsNullOrEmpty(messages[numberOfMessages - 1]);

                        for (var i = 0; i < numberOfMessages - 1; ++i)
                        {
                            var message = messages[i];
                            
                            MessageReceivedEvent?.Invoke(_messageConverter.ConvertStringToMessage(message));

                        }
                        state.Sb.Clear();
                        if (!wholeMessages)
                        {
                            state.Sb.Append(messages[numberOfMessages - 1]);
                        }

                        _receiveDone.Set();

                    } else
                    {
                        client.BeginReceive(state.Buffer, 0, CommunicationStateObject.BufferSize, 0, ReceiveCallback, state);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        

        public void Send(IMessage message)
        {
            _connectDoneForSend.WaitOne();
            var byteData = Encoding.ASCII.GetBytes(_messageConverter.ConvertMessageToString(message) +  CommunicationStateObject.EtbByte);
            try
            {
                _client?.BeginSend(byteData, 0, byteData.Length, 0, SendCallback, _client);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                var bytesSent = _client.EndSend(ar);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void SetupClient(Action<IMessage> messageHandler)
        {
            this.MessageReceivedEvent += messageHandler;
        }
    }
}
