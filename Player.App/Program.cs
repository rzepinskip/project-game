﻿using Common;

namespace Player.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var player = new Player();
            player.InitializePlayer("game", TeamColor.Blue, PlayerType.Leader);
            player.CommunicationClient.Send(player.GetNextRequestMessage());
        }
    }
}
