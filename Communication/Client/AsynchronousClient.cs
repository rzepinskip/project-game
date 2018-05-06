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

        public void Connect(Action<IMessage> messageHandler)
        {
            _connector.Connect(messageHandler);
        }

        public void Send(IMessage message)
        {
            _connector.ConnectFinalized.WaitOne();
            var byteData = Encoding.ASCII.GetBytes(message.SerializeToXml() + Constants.EtbByte);
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