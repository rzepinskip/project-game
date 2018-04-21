using System;
using System.Threading;

namespace GameSimulation
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var simulation = new GameSimulation("Resources/ExampleAdvancedConfig.xml");
            simulation.StartSimulation();
            while (true)
            {
                var boardVisualizer = new BoardVisualizer();
                for (var i = 0;; i++)
                {
                    if (simulation.GameFinished)
                        break;

                    Thread.Sleep(200);
                    boardVisualizer.VisualizeBoard(simulation.GameMaster.Board);
                    Console.WriteLine(i);
                }

                Console.WriteLine($"Game finished - team {simulation.Winners} won!");
                Thread.Sleep(1000);
            }
        }
    }
}