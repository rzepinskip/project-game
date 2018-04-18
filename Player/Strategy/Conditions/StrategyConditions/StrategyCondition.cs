using Messaging.Requests;
using Player.Strategy.StateInfo;
using Player.Strategy.States;
using Player.Strategy.States.StrategyStates;

namespace Player.Strategy.Conditions.StrategyConditions
{
    public abstract class StrategyCondition : ICondition
    {
        protected StrategyCondition(StrategyInfo strategyInfo)
        {
            StrategyInfo = strategyInfo;
        }

        protected StrategyInfo StrategyInfo { get; }

        public abstract bool CheckCondition();
        public abstract StrategyState GetNextState(StrategyState fromStrategyState);
        public abstract Request GetNextMessage(StrategyState fromStrategyState);
    }
}