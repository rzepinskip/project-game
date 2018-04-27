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
using CommunicationServer.Communication;

namespace CommunicationServer
{
    public class AsynchronousSocketListener : IServer
    {
        private readonly IMessageConverter _messageConverter;
        private readonly int _keepAliveTimeMiliseconds;
        private Timer _checkKeepAliveTimer;
        private readonly IAccepter _accepter;
        public AsynchronousSocketListener(IMessageConverter messageConverter,
            IAccepter accepter, int keepAliveTimeMiliseconds)
        {
            _keepAliveTimeMiliseconds = keepAliveTimeMiliseconds;
            _messageConverter = messageConverter;
            _accepter = accepter;
            _checkKeepAliveTimer = new Timer(KeepAliveCallback, _accepter.AgentToCommunicationHandler, 0, _keepAliveTimeMiliseconds/2);
        }

        public void StartListening()
        {
            _accepter.StartListening();
        }

        public void Send(IMessage message, int id)
        {
            var byteData = _messageConverter.ConvertMessageToBytes(message, CommunicationStateObject.EtbByte);
            var findResult = _accepter.AgentToCommunicationHandler.TryGetValue(id, out var handler);
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
            var dictionary = (Dictionary<int, TcpCommunicationTool>)state;
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