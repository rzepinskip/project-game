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
        private readonly ClientManager _clientManager;
        public AsynchronousSocketListener(IAccepter accepter)
        {
            _accepter = accepter;
            _clientManager = new ClientManager(_accepter.AgentToCommunicationHandler);
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
                /// [ERROR_STATE]
                /// BeginSend throws throws SocketException (error when attempting to access socket)
                /// and ObjectDisposedException (when socket is closed)
                /// ArgumentOutOfRangeException
                ///    offset is less than the length of buffer.
                ///-or -
                ///    size is less than 0.
                ///- or -
                ///    size is greater than the length of buffer minus the value of the offset parameter.
                /// 
                /// handle as connection error (shutdown or reconnection)

                Console.WriteLine(e.ToString());
            }
        }

        public void MarkClientAsPlayer(int id)
        {
            _clientManager.MarkClientAsPlayer(id);
        }

        public void MarkClientAsGameMaster(int id)
        {
            _clientManager.MarkClientAsGameMaster(id);
        }

        
    }
}