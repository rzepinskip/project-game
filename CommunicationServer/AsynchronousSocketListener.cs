using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Common;
using Common.Communication;
using Common.Interfaces;

namespace CommunicationServer
{
    public class AsynchronousSocketListener : ICommunicationServer
    {
        public event Action<IMessage, int> MessageReceivedEvent;
        public ManualResetEvent readyForAccept = new ManualResetEvent(false);

        //public Socket GmSocket;
        public Dictionary<int, Socket> _agentToSocket;
        public Dictionary<int, int> _playerIdToGameId;
        public Dictionary<int, GameInfo> _gameIdToGameInfo;

        private int _counter;

        private readonly IMessageConverter _messageConverter;

        public AsynchronousSocketListener(IMessageConverter messageConverter)
        {
            _messageConverter = messageConverter;
            _counter = 0;
        }

        public void StartListening()
        {
            _agentToSocket = new Dictionary<int, Socket>();

            var ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            var ipAddress = ipHostInfo.AddressList[0];
            var localEndPoint = new IPEndPoint(ipAddress, 11000);

            var listener = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                while (true)
                {
                   
                    readyForAccept.Reset();

                    Console.WriteLine("Waiting for a connection...");
                    listener.BeginAccept(AcceptCallback, listener);

                    readyForAccept.WaitOne();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            readyForAccept.Set();

            var listener = (Socket)ar.AsyncState;
            var handler = listener.EndAccept(ar);

            var state = new CommunicationStateObject(handler, _counter);

            _agentToSocket.Add(_counter++, handler);

            StartReading(state);
        }

        private void StartReading(CommunicationStateObject state)
        {
            while (true)
            {
                state.MessageProcessed.Reset();

                Receive(state);

                state.MessageProcessed.WaitOne();
            }
        }

        private void Receive(CommunicationStateObject state)
        {

            try
            {
                state.WorkSocket.BeginReceive(state.Buffer, 0, CommunicationStateObject.BufferSize, 0,
                    ReadCallback, state);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void ReadCallback(IAsyncResult ar)
        {
            Console.WriteLine("in callback");
            var content = string.Empty;
            var state = (CommunicationStateObject)ar.AsyncState;
            var handler = state.WorkSocket;
            var bytesRead = 0;
            try
            {
                bytesRead = handler.EndReceive(ar);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            if (bytesRead > 0)
            {
                state.Sb.Append(Encoding.ASCII.GetString(state.Buffer, 0, bytesRead));
                content = state.Sb.ToString();
                if (content.IndexOf(CommunicationStateObject.ETBByte) > -1)
                {
                    var messages = content.Split(CommunicationStateObject.ETBByte);
                    var numberOfMessages = messages.Length;
                    var wholeMessages = String.IsNullOrEmpty(messages[numberOfMessages - 1]);

                    for (var i = 0; i < numberOfMessages - 1; ++i)
                    {
                        var message = messages[i];
                        //Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",
                        //    message.Length, message);
                        MessageReceivedEvent?.Invoke(_messageConverter.ConvertStringToMessage(message), state.SocketID);

                    }
                    state.Sb.Clear();
                    if (!wholeMessages)
                    {
                        state.Sb.Append(messages[numberOfMessages - 1]);
                    }

                    state.MessageProcessed.Set();
                } else
                {
                    handler.BeginReceive(state.Buffer, 0, CommunicationStateObject.BufferSize, 0,
                        ReadCallback, state);
                }
            }
        }

        public void Send(IMessage message, int id)
        {
            var byteData = Encoding.ASCII.GetBytes(_messageConverter.ConvertMessageToString(message) + CommunicationStateObject.ETBByte);
            var findResult = _agentToSocket.TryGetValue(id, out var handler);
            if (!findResult)
            {
                throw new Exception("Non exsistent socket id");
            }
            try
            {
                handler?.BeginSend(byteData, 0, byteData.Length, 0,
                    SendCallback, handler);
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
                var handler = (Socket)ar.AsyncState;
                var bytesSent = handler.EndSend(ar);
                //Console.WriteLine("Sent {0} bytes to client.", bytesSent);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void SetupServer(Action<IMessage, int> messageHandler)
        {
            MessageReceivedEvent += messageHandler;
        }

        public IEnumerable<GameInfo> GetGames()
        {
            return this.
        }

        public int GetGameId(string gameName)
        {
            throw new NotImplementedException();
        }
    }
}