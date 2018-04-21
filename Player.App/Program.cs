using System.Threading;
using Common;

namespace Player.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var player = new Player();
            player.InitializePlayer(TeamColor.Blue);
            Thread.Sleep(500);
            player.CommunicationClient.Send(player.GetNextRequestMessage());
        }
    }
}
