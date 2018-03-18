using System;
using System.Threading;

namespace GameSimulation
{
    class Program
    {
        static void Main(string[] args)
        {
            var iterations = 1000;
            var simulation = new GameSimulation(iterations);
            simulation.StartSimulation();

            var boardVisualizer = new BoardVisualizer();
            for (int i = 0; i < iterations*10; i++)
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
