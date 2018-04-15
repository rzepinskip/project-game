using System.Threading;
using Common.Interfaces;

namespace CommunicationServer
{

    public class GameCommunicationServer
    {
        private readonly ICommunicationServer _listener;
        public GameCommunicationServer()
        {
            _listener = new AsynchronousSocketListener(new CommunicationServerConverter(), 1000000000);
            _listener.SetupServer(HandleMessage);
            new Thread(() => _listener.StartListening()).Start();
        }

        public void HandleMessage(IMessage message, int i)
        {
            message.Process(_listener, i);
        }
    }
}
