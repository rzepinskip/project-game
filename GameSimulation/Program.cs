using System;
using System.Collections.Generic;
using System.Threading;
using Common;
using Player.StrategyGroups;

namespace GameSimulation
{
    internal class Program
    {
        private static VerboseLogger _logger;
        private static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            var strategyGroups = new Dictionary<TeamColor, StrategyGroup>
            {
                {
                    TeamColor.Blue, StrategyGroupFactory.Create(StrategyGroupType.Basic)
                },
                {
                    TeamColor.Red, StrategyGroupFactory.Create(StrategyGroupType.Basic)
                }
            };
            var simulation = new GameSimulation("../../../../ExampleConfig.xml", strategyGroups);

            _logger = simulation.GameMaster.VerboseLogger;

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

                if (simulation.GameFinished)
                {
                    Console.WriteLine($"Game finished - team {simulation.Winners} won!");
                    simulation.GameFinished = false;
                }

                Thread.Sleep(1000);
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            ApplicationFatalException.HandleFatalException(args, _logger);
        }
    }
}