using Common;
using Messaging.Serialization;

namespace Player.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var player = new Player(MessageSerializer.Instance);
            player.InitializePlayer("game", TeamColor.Blue, PlayerType.Leader);
            player.CommunicationClient.Send(player.GetNextRequestMessage());
        }
    }
}
