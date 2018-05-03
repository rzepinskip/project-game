using System;
using System.Text;
using Common.Interfaces;
using Communication.Exceptions;

namespace Communication.Client
{
    public class AsynchronousClient : IClient
    {
        private readonly IConnector _connector;
        public AsynchronousClient(IConnector connector)
        {
            _connector = connector;
        }

        public void Connect()
        {
            _connector.Connect();
        }

        public void Send(IMessage message)
        {
            _connector.ConnectFinalized.WaitOne();
            var byteData = Encoding.ASCII.GetBytes(message.SerializeToXml() + CommunicationState.EtbByte);
            try
            {
                _connector.TcpConnection.Send(byteData);
            }
            catch (Exception e)
            {
                throw new ConnectionException("Unable to send message", e);
            }
        }
    }
}