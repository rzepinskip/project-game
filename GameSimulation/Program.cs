﻿using System;
using System.Diagnostics;
using System.Threading;

namespace GameSimulation
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            var simulation = new GameSimulation("Resources/ExampleAdvancedConfig.xml");
            simulation.StartSimulation();
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

                    throw new NotImplementedException();
                }



                if (simulation.GameFinished)
                {
                    Console.WriteLine($"Game finished - team {simulation.Winners} won!");
                    simulation.GameFinished = false;
                }



                Thread.Sleep(1000);
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Debug.WriteLine("GLOBAL EXCEPTION");
            Console.WriteLine("GLOBAL EXCEPTION");
        }
    }
}