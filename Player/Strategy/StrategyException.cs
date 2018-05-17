using System;
using Player.Strategy.States;

namespace Player.Strategy
{
    public class StrategyException : Exception
    {
        public StrategyException(string message, IExceptionContentProvider context)
            : base(message + '\n' + context.GetExceptionInfo())
        {
        }

        public StrategyException(string message, IExceptionContentProvider context,
            BaseState currentStrategyState)
            : base(string.Join("\n", message, "StrategyState: " + currentStrategyState, context))
        {
        }
    }
}