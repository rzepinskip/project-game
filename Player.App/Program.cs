using System;
using System.Net;
using BoardGenerators.Loaders;
using Common;
using GameMaster.Configuration;
using Messaging.Serialization;
using Mono.Options;

namespace Player.App
{
    class Program
    {
        static void Usage(OptionSet options)
        {
            Console.WriteLine("Usage:");
            options.WriteOptionDescriptions(Console.Out);
        }

        static void Main(string[] args)
        {
            bool teamFlag = false, roleFlag = false, addressFlag = false;
            var port = default(int);
            var gameConfigPath = default(string);
            var address = default(IPAddress);
            var gameName = default(string);
            var team = default(TeamColor);
            var role = default(PlayerType);

            var options = new OptionSet {
                { "port=", "port number", (int p) => port = p },
                { "conf=",  "configuration filename", c => gameConfigPath=c},
                { "address=", "server adress or hostname", a => addressFlag = IPAddress.TryParse(a, out address)},
                { "game=", "name of the game", g => gameName = g},
                { "team=", "red|blue", t =>  teamFlag = TeamColor.TryParse(t, true, out team)},
                { "role=", "leader|player", r => roleFlag = PlayerType.TryParse(r, true, out role)}
            };

            options.Parse(args);

            if (port == default(int) || gameConfigPath == default(string) || gameName == default(string) ||!addressFlag || !teamFlag || !roleFlag)
                Usage(options);


            var configLoader = new XmlLoader<GameConfiguration>();
            var config = configLoader.LoadConfigurationFromFile(gameConfigPath);

            var player = new Player(MessageSerializer.Instance, port, (int)config.KeepAliveInterval, address);
            player.InitializePlayer(gameName, team, role);
            player.CommunicationClient.Send(player.GetNextRequestMessage());
        }
    }
}
