﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Common.Interfaces;
using Communication;
using CommunicationServer.Accepters;

namespace CommunicationServer
{
    public class AsynchronousSocketListener : IAsynchronousSocketListener
    {
        private readonly IAccepter _accepter;
        private readonly int _keepAliveTimeMiliseconds;
        private Timer _checkKeepAliveTimer;

        public AsynchronousSocketListener(IAccepter accepter, int keepAliveTimeMiliseconds)
        {
            _keepAliveTimeMiliseconds = keepAliveTimeMiliseconds;
            _accepter = accepter;
            _checkKeepAliveTimer = new Timer(KeepAliveCallback, _accepter.AgentToCommunicationHandler, 0,
                _keepAliveTimeMiliseconds / 2);
        }

        public void StartListening()
        {
            _accepter.StartListening();
        }

        public void Send(IMessage message, int id)
        {
            var byteData =  Encoding.ASCII.GetBytes(message.SerializeToXml() + CommunicationState.EtbByte);
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
            var dictionary = (Dictionary<int, TcpConnection>) state;
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