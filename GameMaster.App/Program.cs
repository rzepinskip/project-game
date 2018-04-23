using System;
using System.Threading;
using BoardGenerators.Loaders;
using Common;
using Common.Interfaces;
using GameMaster.Configuration;

namespace GameMaster.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var gameConfigPath = "Resources/ExampleConfig.xml";
            var configLoader = new XmlLoader<GameConfiguration>();
            var config = configLoader.LoadConfigurationFromFile(gameConfigPath);

            var gm = new GameMaster(config);

            while (true)
            {
                var boardVisualizer = new BoardVisualizer();
                for (var i = 0; ; i++)
                {
                    Thread.Sleep(200);
                    boardVisualizer.VisualizeBoard(gm.Board);
                    Console.WriteLine(i);
                }
            }
        }
    }
}
