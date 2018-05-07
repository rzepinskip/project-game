using System.Collections.Generic;
using Common.Interfaces;
using Player.Strategy.Conditions;
using Player.Strategy.StateInfo;

namespace Player.Strategy.States.StrategyStates
{
    public abstract class StrategyState : BaseState, ILoggable
    {
        protected StrategyState(StrategyInfo strategyInfo)
        {
            stateInfo = strategyInfo;
            transitionConditions = new List<ICondition>();
        }

        protected StrategyState()
        {
            transitionConditions = new List<ICondition>();
        }

        public string ToLog()
        {
            return GetType() + stateInfo.ToLog();
        }
    }
}