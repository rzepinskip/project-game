using System;
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

        public AsynchronousSocketListener(IAccepter accepter)
        {
            _accepter = accepter;
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

        
    }
}