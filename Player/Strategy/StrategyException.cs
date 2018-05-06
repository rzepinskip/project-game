using System;

namespace Player.Strategy
{
    public class StrategyException : Exception
    {
        public StrategyException(string message, IExceptionContentProvider context)
            : base(message + '\n' + context.GetExceptionInfo())
        {
        }

        public StrategyException(string message, IExceptionContentProvider currentStrategyState,
            IExceptionContentProvider context)
            : base(string.Join("\n", message, "StrategyState: " + currentStrategyState, context))
        {
        }
    }
}