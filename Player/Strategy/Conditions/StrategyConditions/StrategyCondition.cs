using Common.Interfaces;
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
        public abstract BaseState GetNextState(BaseState fromStrategyState);
        public abstract IMessage GetNextMessage(BaseState fromStrategyState);
        public abstract bool ReturnsMessage(BaseState fromStrategyState);
    }
}