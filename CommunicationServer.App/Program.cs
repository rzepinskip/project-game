using System;
using BoardGenerators.Loaders;
using GameMaster.Configuration;
using Messaging.Serialization;
using Mono.Options;

namespace CommunicationServer.App
{
    class Program
    {
        private static void Usage(OptionSet options)
        {
            Console.WriteLine("Options:");
            options.WriteOptionDescriptions(Console.Out);
        }

        static void Main(string[] args)
        {
            var port = default(int);
            var gameConfigPath = default(string);

            var options = new OptionSet {
                { "port=", "port number", (int p) => port = p },
                { "conf=",  "configuration filename", c => gameConfigPath=c},
            };

            options.Parse(args);

            if (port == default(int) || gameConfigPath == default(string))
                Usage(options);

            var configLoader = new XmlLoader<GameConfiguration>();
            var config = configLoader.LoadConfigurationFromFile(gameConfigPath);

            var cs = new CommunicationServer(MessageSerializer.Instance, config.KeepAliveInterval, port);
        }
    }
}
