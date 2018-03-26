using System;
using Player.Strategy.States;

namespace Player.Strategy
{
    public class StrategyException : Exception
    {
        public StrategyException(string message, StrategyInfo context)
            : base(message + '\n' + context)
        {
        }

        public StrategyException(string message, State currentState, StrategyInfo context)
            : base(string.Join("\n", message, "State: " + currentState, context))
        {
        }
    }
}