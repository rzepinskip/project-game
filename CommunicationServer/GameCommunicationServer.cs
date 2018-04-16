using System;
using System.Diagnostics;
using System.Threading;
using Common.Interfaces;
using NLog;

namespace CommunicationServer
{

    public class GameCommunicationServer
    {
        private readonly ICommunicationServer _listener;
        private static ILogger _logger = LogManager.GetCurrentClassLogger();
        public GameCommunicationServer()
        {
            _listener = new AsynchronousSocketListener(new CommunicationServerConverter(), 1000000000);
            _listener.SetupServer(HandleMessage);
            new Thread(() => _listener.StartListening()).Start();
        }

        public void HandleMessage(IMessage message, int i)
        {
            //Debug.WriteLine("CS Message received from: " + i)
            _logger.Info(message.ToString() + " from  id: " + i);
            message.Process(_listener, i);
        }

        public void HandleCallback(IAsyncResult ar)
        {

        }
    }
}
