﻿using System;
using Common;
using BoardGenerators.Loaders;
using GameMaster.Configuration;
using Messaging.Serialization;
using NLog;
using Mono.Options;

namespace CommunicationServer.App
{
    class Program
    {
        private static ILogger _logger;

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            var cs = CreateCommunicationServerFrom(args);
            _logger = CommunicationServer.Logger;
        }

        private static CommunicationServer CreateCommunicationServerFrom(string[] args)
        {
            var port = default(int);
            var gameConfigPath = default(string);

            var options = new OptionSet
            {
                {"port=", "port number", (int p) => port = p},
                {"conf=", "configuration filename", c => gameConfigPath = c},
            };

            options.Parse(args);

            if (port == default(int) || gameConfigPath == default(string))
                Usage(options);

            var configLoader = new XmlLoader<GameConfiguration>();
            var config = configLoader.LoadConfigurationFromFile(gameConfigPath);

            return new CommunicationServer(MessageSerializer.Instance, config.KeepAliveInterval, port);
        }

        private static void Usage(OptionSet options)
        {
            Console.WriteLine("Options:");
            options.WriteOptionDescriptions(Console.Out);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            UnhandledApplicationException.HandleGlobalException(args, _logger);
        }
    }
}
