using System;
using BoardGenerators.Loaders;
using Common;
using GameMaster.Configuration;
using Messaging.Serialization;
using Mono.Options;
using NLog;

namespace CommunicationServer.App
{
    internal class Program
    {
        private static ILogger _logger;

        private static void Main(string[] args)
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
                {"conf=", "configuration filename", c => gameConfigPath = c}
            };

            options.Parse(args);

            if (port == default(int) || gameConfigPath == default(string))
                Usage(options);

            var configLoader = new XmlLoader<GameConfiguration>();
            var config = configLoader.LoadConfigurationFromFile(gameConfigPath);
            var keepAliveInterval = TimeSpan.FromMilliseconds((int) config.KeepAliveInterval);

            return new CommunicationServer(MessageSerializer.Instance, keepAliveInterval, port);
        }

        private static void Usage(OptionSet options)
        {
            Console.WriteLine("Options:");
            options.WriteOptionDescriptions(Console.Out);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            ApplicationFatalException.HandleFatalException(args, _logger);
        }
    }
}