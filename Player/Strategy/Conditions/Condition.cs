using Common.Interfaces;
using Messaging.Requests;
using Player.Strategy.StateInfo;
using Player.Strategy.States;
using Player.Strategy.States.StrategyStates;

namespace Player.Strategy.Conditions
{
    public abstract class Condition : ICondition
    {
        protected Condition(StrategyInfo strategyInfo)
        {
            StrategyInfo = strategyInfo;
        }

        protected StrategyInfo StrategyInfo { get; }

        public abstract bool CheckCondition();
        public abstract BaseState GetNextState(BaseState fromStrategyState);
        public abstract IMessage GetNextMessage(BaseState fromStrategyState);
    }
}