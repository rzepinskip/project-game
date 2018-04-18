using System;
using Player.Strategy.StateInfo;
using Player.Strategy.States;
using Player.Strategy.States.StrategyStates;

namespace Player.Strategy
{
    public class StrategyException : Exception
    {
        public StrategyException(string message, IExceptionContentProvider context)
            : base(message + '\n' + context.getExceptionInfo())
        {
        }

        public StrategyException(string message, StrategyState currentStrategyState, IExceptionContentProvider context)
            : base(string.Join("\n", message, "StrategyState: " + currentStrategyState, context))
        {
        }
    }
}