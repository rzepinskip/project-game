using System;
using System.Threading;
using Common;
using Player.StrategyGroup;

namespace GameSimulation
{
    internal class Program
    {
        public Program(StrategyGroup strategyGroup)
        {
            _strategyGroup = strategyGroup;
        }

        private static VerboseLogger _logger;
        private static StrategyGroup _strategyGroup;

        private static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            var simulation = new GameSimulation("../../../../ExampleConfig.xml", _strategyGroup);

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