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
        private readonly IConnecter _connecter;
        private readonly IMessageConverter _messageConverter;

        public AsynchronousClient(IConnecter connecter, IMessageConverter messageConverter)
        {
            _connecter = connecter;
            _messageConverter = messageConverter;
        }

        public void StartClient()
        {
            _connecter.Connect();  
        }
        public void Send(IMessage message)
        {
            _connecter.ConnectDoneForSend.WaitOne();
            var byteData = Encoding.ASCII.GetBytes(_messageConverter.ConvertMessageToString(message) +  CommunicationStateObject.EtbByte);
            try
            {
                _connecter.ClientTcpCommunicationTool.Send(byteData);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}

