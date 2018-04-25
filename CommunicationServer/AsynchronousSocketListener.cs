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
using Messaging.Communication;

namespace CommunicationServer
{
    public class AsynchronousSocketListener : IServer
    {
        private Action<IMessage, int> MessageReceivedEvent;
        private readonly ManualResetEvent _readyForAccept = new ManualResetEvent(false);
        private readonly Dictionary<int, CommunicationHandler> _agentToCommunicationStateObject;

        private int _counter;
        private readonly IMessageConverter _messageConverter;
        private readonly int _keepAliveTimeMiliseconds;
        private Timer _checkKeepAliveTimer;

        public AsynchronousSocketListener(IMessageConverter messageConverter, Action<IMessage, int> messageHandler, int keepAliveTimeMiliseconds)
        {
            _keepAliveTimeMiliseconds = keepAliveTimeMiliseconds;
            _messageConverter = messageConverter;
            MessageReceivedEvent = messageHandler;

            //Only for gameSimulation, the GM must have ID = -1 to get request queues working properly
            _counter = 0;
            _agentToCommunicationStateObject = new Dictionary<int, CommunicationHandler>();
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
            var state = new ServerCommunicationHandler(handler, _counter, new CommunicationServerConverter(), MessageReceivedEvent);
            _agentToCommunicationStateObject.Add(_counter++, state);
            StartReading(state);
        }

        private void StartReading(CommunicationHandler handler)
        {
            while (true)
                handler.Receive();
        }


       
        public void Send(IMessage message, int id)
        {
            var byteData = _messageConverter.ConvertMessageToBytes(message, CommunicationStateObject.EtbByte);
            var findResult = _agentToCommunicationStateObject.TryGetValue(id, out var handler);
            if (!findResult)
                throw new Exception("Non exsistent socket id");

            try
            {
                handler.Send(byteData);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }


        public void KeepAliveCallback(object state)
        {
            var dictionary = (Dictionary<int, CommunicationHandler>)state;
            var currentTime = DateTime.Now.Ticks;
            foreach (var csStateObject in dictionary.Values)
            {
                var elapsedTicks = currentTime - csStateObject.State.LastMessageReceivedTicks;
                var elapsedSpan = new TimeSpan(elapsedTicks);

                if (elapsedSpan.Milliseconds > _keepAliveTimeMiliseconds)
                    csStateObject.CloseSocket();
            }
        }
    }
}