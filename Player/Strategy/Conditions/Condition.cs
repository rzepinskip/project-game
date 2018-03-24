using Player.Strategy.States;
using Shared.GameMessages;

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
        public abstract GameMessage GetNextMessage(State fromState);
    }
}