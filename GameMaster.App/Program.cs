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

namespace GameMaster.App
{
    internal class Program
    {
        private static VerboseLogger _logger;
        private static string _finishedGameMessage = "";

        private static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            var gm = CreateGameMasterFrom(args);
            gm.GameFinished += GenerateNewFinishedGameMessage;
            _logger = gm.VerboseLogger;
            Console.Title = "Game Master";

            while (true)
            {
                var boardVisualizer = new BoardVisualizer();
                for (var i = 0;; i++)
                {
                    Thread.Sleep(200);
                    boardVisualizer.VisualizeBoard(gm.Board);
                    Console.WriteLine(i);
                    Console.WriteLine(_finishedGameMessage);
                }
            }
        }

        private static void GenerateNewFinishedGameMessage(object sender, GameFinishedEventArgs e)
        {
            var gameMaster = sender as GameMaster;
            gameMaster.LogGameResults(e.Winners);
            _finishedGameMessage = "Last game winners: " + (e.Winners == TeamColor.Red ? "Red " : "Blue");
        }

        private static GameMaster CreateGameMasterFrom(IEnumerable<string> parameters)
        {
            var addressFlag = false;
            var port = default(int);
            var gameConfigPath = default(string);
            var ipAddress = default(IPAddress);
            var gameName = default(string);
            var loggingMode = LoggingMode.NonVerbose;

            var options = new OptionSet
            {
                {"port=", "port number", (int p) => port = p},
                {"conf=", "configuration filename", c => gameConfigPath = c},
                {"address=", "server adress or hostname", a => addressFlag = IPAddress.TryParse(a, out ipAddress)},
                {"game=", "name of the game", g => gameName = g},
                {"verbose:", "logging mode", v => loggingMode = LoggingMode.Verbose }
            };

            options.Parse(parameters);

            if (!addressFlag)
            {
                addressFlag = true;
                var ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                ipAddress = ipHostInfo.AddressList[0];
            }

            if (port == default(int) || gameConfigPath == default(string) || gameName == default(string) ||
                !addressFlag)
                Usage(options);

            var configLoader = new XmlLoader<GameConfiguration>();
            var config = configLoader.LoadConfigurationFromFile(gameConfigPath);

            var communicationClient = new AsynchronousCommunicationClient(new IPEndPoint(ipAddress, port), TimeSpan.FromMilliseconds((int) config.KeepAliveInterval), MessageSerializer.Instance);

            return new GameMaster(config, communicationClient, gameName, new ErrorsMessagesFactory(), loggingMode);
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