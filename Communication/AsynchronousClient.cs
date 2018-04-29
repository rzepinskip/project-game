using System;
using System.Text;
using Common.Interfaces;

namespace Communication
{
    public class AsynchronousClient : IClient
    {
        private readonly IConnector _connector;
        private readonly IMessageConverter _messageConverter;

        public AsynchronousClient(IConnector connector, IMessageConverter messageConverter)
        {
            _connector = connector;
            _messageConverter = messageConverter;
        }

        public void Connect()
        {
            _connector.Connect();
        }

        public void Send(IMessage message)
        {
            _connector.ConnectFinalized.WaitOne();
            var byteData =
                Encoding.ASCII.GetBytes(_messageConverter.ConvertMessageToString(message) +
                                        CommunicationState.EtbByte);
            try
            {
                _connector.TcpConnection.Send(byteData);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}