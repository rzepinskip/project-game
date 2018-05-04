using System;
using System.Threading;
using GameMaster;
using Messaging.Serialization;

namespace GameSimulation
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var simulation = new GameSimulation("Resources/ExampleConfig.xml");
            simulation.StartSimulation();


            var xmlSerializer = new ExtendedXmlSerializer(string.Empty);

            while (true)
            {
                var boardVisualizer = new BoardVisualizer();
                for (var i = 0; ; i++)
                {
                    if (simulation.GameFinished)
                        break;

                    Thread.Sleep(200);
                    boardVisualizer.VisualizeBoard(simulation.GameMaster.Board);
                    Console.WriteLine(i);
                    if (i == 22)
                    {
                        var xml = xmlSerializer.SerializeToXml(simulation.GameMaster.Board);
                        var board = xmlSerializer.DeserializeFromXml<GameMasterBoard>(xml);
                    }
                }

                if (simulation.GameFinished)
                {
                    Console.WriteLine($"Game finished - team {simulation.Winners} won!");
                    simulation.GameFinished = false;
                }

                Thread.Sleep(1000);
            }
        }
    }
}