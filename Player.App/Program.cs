using System;
using Common;
using Messaging.Serialization;
using NLog;

namespace Player.App
{
    class Program
    {
        private static ILogger _logger;

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            var player = new Player(MessageSerializer.Instance);
            _logger = player.Logger;
            player.InitializePlayer("game", TeamColor.Blue, PlayerType.Leader);
            player.CommunicationClient.Send(player.GetNextRequestMessage());
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            GlobalException.HandleGlobalException(args, _logger);
        }
    }
}
