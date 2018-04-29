using Messaging.Serialization;

namespace CommunicationServer.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var cs = new GameCommunicationServerCommunicator(MessageSerializer.Instance);
        }
    }
}
