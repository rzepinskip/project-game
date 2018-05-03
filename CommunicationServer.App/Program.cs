using Messaging.Serialization;

namespace CommunicationServer.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var cs = new CommunicationServer(MessageSerializer.Instance);
        }
    }
}
