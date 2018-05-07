using System;
using System.Collections.Generic;
using System.Net;
using BoardGenerators.Loaders;
using Common;
using Communication.Client;
using GameMaster.Configuration;
using Messaging.Serialization;
using Mono.Options;
using NLog;

namespace Player.App
{
    internal class Program
    {
        private static ILogger _logger;

        private static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            var player = CreatePlayerFrom(args);

            _logger = player.Logger;
            player.CommunicationClient.Send(player.GetNextRequestMessage());
        }

        private static Player CreatePlayerFrom(IEnumerable<string> parameters)
        {
            bool teamFlag = false, roleFlag = false, addressFlag = false;
            var ipAddress = default(IPAddress);
            var port = default(int);
            var gameConfigPath = default(string);
            var gameName = default(string);
            var team = default(TeamColor);
            var role = default(PlayerType);

            var options = new OptionSet
            {
                {"port=", "port number", (int p) => port = p},
                {"conf=", "configuration filename", c => gameConfigPath = c},
                {"address=", "server adress or hostname", a => addressFlag = IPAddress.TryParse(a, out ipAddress)},
                {"game=", "name of the game", g => gameName = g},
                {"team=", "red|blue", t => teamFlag = Enum.TryParse(t, true, out team)},
                {"role=", "leader|player", r => roleFlag = Enum.TryParse(r, true, out role)}
            };

            options.Parse(parameters);

            if (!addressFlag)
            {
                addressFlag = true;
                var ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                address = ipHostInfo.AddressList[0];
            }

            if (port == default(int) || gameConfigPath == default(string) || gameName == default(string) ||
                !addressFlag || !teamFlag || !roleFlag)
                Usage(options);


            var configLoader = new XmlLoader<GameConfiguration>();
            var config = configLoader.LoadConfigurationFromFile(gameConfigPath);

            var communicationClient = new AsynchronousClient(MessageSerializer.Instance, new IPEndPoint(ipAddress, port),
                TimeSpan.FromMilliseconds((int) config.KeepAliveInterval));

            var player = new Player(communicationClient, gameName, team, role);

            return player;
        }

        private static void Usage(OptionSet options)
        {
            Console.WriteLine("Usage:");
            options.WriteOptionDescriptions(Console.Out);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            ApplicationFatalException.HandleFatalException(args, _logger);
        }
    }
}