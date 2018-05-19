using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using BoardGenerators.Loaders;
using Common;
using Communication.Client;
using GameMaster.Configuration;
using Messaging.ErrorsMessages;
using Messaging.Serialization;
using Mono.Options;
using NLog;
using Player.StrategyGroups;

namespace Player.App
{
    internal class Program
    {
        private static VerboseLogger _logger;

        private static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            var player = CreatePlayerFrom(args);

            _logger = player.VerboseLogger;
            if (true)
            {
                var boardVisualizer = new BoardVisualizer();
                for (var i = 0;; i++)
                {
                    if (player.PlayerBoard != null)
                    {
                        Thread.Sleep(200);
                        boardVisualizer.VisualizeBoard(player.PlayerBoard, player.Id);
                        Console.WriteLine(i);
                    }
                    else
                    {
                        Thread.Sleep(2000);
                    }
                }
            }
        }

        private static Player CreatePlayerFrom(IEnumerable<string> parameters)
        {
            bool teamFlag = false, roleFlag = false, addressFlag = false, strategyFlag;
            var ipAddress = default(IPAddress);
            var port = default(int);
            var gameConfigPath = default(string);
            var gameName = default(string);
            var team = default(TeamColor);
            var role = default(PlayerType);
            var loggingMode = LoggingMode.NonVerbose;
            var strategyGroupTypeFlag = true;
            var strategyGroupType = StrategyGroupType.Primitive;

            var options = new OptionSet
            {
                {"port=", "port number", (int p) => port = p},
                {"conf=", "configuration filename", c => gameConfigPath = c},
                {"address=", "server adress or hostname", a => addressFlag = IPAddress.TryParse(a, out ipAddress)},
                {"game=", "name of the game", g => gameName = g},
                {"team=", "red|blue", t => teamFlag = Enum.TryParse(t, true, out team)},
                {"role=", "leader|player", r => roleFlag = Enum.TryParse(r, true, out role)},
                {"verbose:", "logging mode", v => loggingMode = LoggingMode.Verbose },
                {"strategy=", "strategy options", s => strategyGroupTypeFlag = Enum.TryParse(s, true, out strategyGroupType) }
            };

            options.Parse(parameters);

            if (!addressFlag)
            {
                addressFlag = true;
                var ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                ipAddress = ipHostInfo.AddressList[0];
            }

            if (port == default(int) || gameConfigPath == default(string) || gameName == default(string) ||
                !addressFlag || !teamFlag || !roleFlag || !strategyGroupTypeFlag)
                Usage(options);


            var configLoader = new XmlLoader<GameConfiguration>();
            var config = configLoader.LoadConfigurationFromFile(gameConfigPath);

            var keepAliveInterval = TimeSpan.FromMilliseconds((int) config.KeepAliveInterval);
            var communicationClient = new AsynchronousCommunicationClient(new IPEndPoint(ipAddress, port), keepAliveInterval,
                MessageSerializer.Instance);
            var strategyGroup = new StrategyGroupFactory().Create(strategyGroupType);
            var player = new Player(communicationClient, gameName, team, role, new ErrorsMessagesFactory(), loggingMode, strategyGroup);

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