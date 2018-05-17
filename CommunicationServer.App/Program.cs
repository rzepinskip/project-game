using System;
using BoardGenerators.Loaders;
using Common;
using GameMaster.Configuration;
using Messaging.ErrorsMessages;
using Messaging.Serialization;
using Mono.Options;

namespace CommunicationServer.App
{
    internal class Program
    {
        private static VerboseLogger _logger;

        private static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            var cs = CreateCommunicationServerFrom(args);
            _logger = CommunicationServer.VerboseLogger;
            Console.Title = "Communication Server";
        }

        private static CommunicationServer CreateCommunicationServerFrom(string[] args)
        {
            var port = default(int);
            var gameConfigPath = default(string);
            var loggingMode = LoggingMode.NonVerbose;

            var options = new OptionSet
            {
                {"port=", "port number", (int p) => port = p},
                {"conf=", "configuration filename", c => gameConfigPath = c},
                {"verbose:", "logging mode", v => loggingMode = LoggingMode.Verbose }
            };

            options.Parse(args);

            if (port == default(int) || gameConfigPath == default(string))
                Usage(options);

            var configLoader = new XmlLoader<GameConfiguration>();
            var config = configLoader.LoadConfigurationFromFile(gameConfigPath);
            var keepAliveInterval = TimeSpan.FromMilliseconds((int) config.KeepAliveInterval);

            return new CommunicationServer(MessageSerializer.Instance, keepAliveInterval, port, new ErrorsMessagesFactory(), loggingMode);
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