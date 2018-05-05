using System;
using System.Net;
using System.Threading;
using BoardGenerators.Loaders;
using Common;
using GameMaster.Configuration;
using Messaging.Serialization;
using Mono.Options;

namespace GameMaster.App
{
    internal class Program
    {
        static void Usage(OptionSet options)
        {
            Console.WriteLine("Usage:");
            options.WriteOptionDescriptions(Console.Out);
        }

        private static void Main(string[] args)
        {

            bool addressFlag = false;
            var port = default(int);
            var gameConfigPath = default(string);
            var address = default(IPAddress);
            var gameName = default(string);

            var options = new OptionSet {
                { "port=", "port number", (int p) => port = p },
                { "conf=",  "configuration filename", c => gameConfigPath=c},
                { "address=", "server adress or hostname", a =>addressFlag = IPAddress.TryParse(a, out address)},
                { "game=", "name of the game", g => gameName = g},
            };

            options.Parse(args);

            if (port == default(int) || gameConfigPath == default(string) || gameName == default(string) || !addressFlag)
                Usage(options);

            var configLoader = new XmlLoader<GameConfiguration>();
            var config = configLoader.LoadConfigurationFromFile(gameConfigPath);

            var gm = new GameMaster(config, MessageSerializer.Instance, port, address);

            while (true)
            {
                var boardVisualizer = new BoardVisualizer();
                for (var i = 0;; i++)
                {
                    Thread.Sleep(200);
                    boardVisualizer.VisualizeBoard(gm.Board);
                    Console.WriteLine(i);
                }
            }
        }
    }
}