using System;
using System.Threading;
using BoardGenerators.Loaders;
using GameMaster.Configuration;
using Messaging.Serialization;

namespace GameMaster.App
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var gameConfigPath = "Resources/ExampleAdvancedConfig.xml";
            var configLoader = new XmlLoader<GameConfiguration>();
            var config = configLoader.LoadConfigurationFromFile(gameConfigPath);

            var gm = new GameMaster(config, MessageSerializer.Instance);

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