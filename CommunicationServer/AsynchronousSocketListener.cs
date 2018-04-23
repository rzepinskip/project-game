using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Common.Communication;
using Common.Interfaces;

namespace CommunicationServer
{
    public class AsynchronousSocketListener : IServer
    {
        private event Action<IMessage, int> _messageReceivedEvent;
        private readonly ManualResetEvent _readyForAccept = new ManualResetEvent(false);

        private readonly Dictionary<int, CommunicationStateObject> _agentToCommunicationStateObject;

        private int _counter;
        private readonly IMessageConverter _messageConverter;
        private readonly int _keepAliveTimeMiliseconds;
        private Timer _checkKeepAliveTimer;

        public AsynchronousSocketListener(IMessageConverter messageConverter, Action<IMessage, int> messageHandler, int keepAliveTimeMiliseconds)
        {
            _keepAliveTimeMiliseconds = keepAliveTimeMiliseconds;
            _messageConverter = messageConverter;
            _messageReceivedEvent += messageHandler;

            //Only for gameSimulation, the GM must have ID = -1 to get request queues working properly
            _counter = 0;
            _agentToCommunicationStateObject = new Dictionary<int, CommunicationStateObject>();
            _checkKeepAliveTimer = new Timer(KeepAliveCallback, _agentToCommunicationStateObject, 0, _keepAliveTimeMiliseconds/2);
        }

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

            //Console.WriteLine("\nPress ENTER to continue...");
            //Console.Read();
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            _readyForAccept.Set();
            var handler = default(Socket);
            var listener = (Socket)ar.AsyncState;
            try
            {
                handler = listener.EndAccept(ar);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Debug.WriteLine("Accepted for " + _counter);
            var state = new CommunicationStateObject(handler);
            _agentToCommunicationStateObject.Add(_counter++, state);
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
                //After closing socket, BeginReceive will throw SocketException which has to be handled
                Console.WriteLine(e.ToString());
            }
        }

        private void ReadCallback(IAsyncResult ar)
        {
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
                if (content.IndexOf(CommunicationStateObject.EtbByte) > -1)
                {
                    var messages = content.Split(CommunicationStateObject.EtbByte);
                    var numberOfMessages = messages.Length;
                    var wholeMessages = string.IsNullOrEmpty(messages[numberOfMessages - 1]);

                    for (var i = 0; i < numberOfMessages - 1; ++i)
                    {
                        var message = messages[i];
                        Debug.WriteLine("Read {0} bytes from socket. \n Data : {1}",
                            message.Length, message);
                        state.LastMessageReceivedTicks = DateTime.Today.Ticks;
                        _messageReceivedEvent?.Invoke(_messageConverter.ConvertStringToMessage(message), _agentToCommunicationStateObject.First( x => x.Value.Equals(state)).Key);

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
            var byteData = _messageConverter.ConvertMessageToBytes(message, CommunicationStateObject.EtbByte);
            var findResult = _agentToCommunicationStateObject.TryGetValue(id, out var handler);
            if (!findResult)
            {
                throw new Exception("Non exsistent socket id");
            }
            try
            {
                handler.WorkSocket?.BeginSend(byteData, 0, byteData.Length, 0,
                    SendCallback, handler.WorkSocket);
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
                Debug.WriteLine("Sent {0} bytes to client.", bytesSent);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void KeepAliveCallback(object state)
        {
            var dictionary = (Dictionary<int, CommunicationStateObject>)state;
            var currentTime = DateTime.Now.Ticks;
            foreach (var csStateObject in dictionary.Values)
            {
                var elapsedTicks = currentTime - csStateObject.LastMessageReceivedTicks;
                var elapsedSpan = new TimeSpan(elapsedTicks);

                if (elapsedSpan.Milliseconds > _keepAliveTimeMiliseconds)
                    CloseSocket(csStateObject.WorkSocket);
            }

        }

        private void CloseSocket(Socket socketToClose)
        {
            if (socketToClose == null)
                return;
            try
            {
                socketToClose.Shutdown(SocketShutdown.Both);
                socketToClose.Close();
            }
            catch (Exception e)
            {
                //
            }
        }
    }
}