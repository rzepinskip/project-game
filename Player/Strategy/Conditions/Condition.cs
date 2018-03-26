using Messaging.Requests;
using Player.Strategy.States;

namespace Player.Strategy.Conditions
{
    public abstract class Condition
    {
        protected Condition(StrategyInfo strategyInfo)
        {
            StrategyInfo = strategyInfo;
        }

        protected StrategyInfo StrategyInfo { get; }

        public abstract bool CheckCondition();
        public abstract State GetNextState(State fromState);
        public abstract Request GetNextMessage(State fromState);
    }
}