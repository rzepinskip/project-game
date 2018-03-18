using System;
using System.Threading;

namespace GameSimulation
{
    class Program
    {
        static void Main(string[] args)
        {
            var simulation = new GameSimulation( "Resources/ExampleAdvancedConfig.xml");
            simulation.StartSimulation();

            var boardVisualizer = new BoardVisualizer();
            for (int i = 0;; i++)
            {
                if (simulation.GameFinished)
                    break;

                Thread.Sleep(200);
                boardVisualizer.VisualizeBoard(simulation.GameMaster.Board);
                Console.WriteLine(i);
            }
            Console.WriteLine("game finished");
        }

    }
}
