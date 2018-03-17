using System;
using Player;
using Shared.BoardObjects;
using System.Collections;
using Shared.GameMessages;
using System.Collections.Generic;
using System.Threading;
using Shared.ResponseMessages;
using Shared;

namespace GameSimulation
{
    class Program
    {
        static void Main(string[] args)
        {
            var iterations = 100;
            var simulation = new GameSimulation(iterations);
            simulation.StartSimulation();

            var boardVisualizer = new BoardVisualizer();
            for (int i = 0; i < iterations*10; i++)
            {
                Thread.Sleep(200);
                boardVisualizer.VisualizeBoard(simulation.GameMaster.Board);
                Console.WriteLine(i);
            }
        }

    }
}
