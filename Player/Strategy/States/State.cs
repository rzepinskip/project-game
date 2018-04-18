using System.Collections.Generic;
using Common.Interfaces;
using Messaging.Requests;
using Player.Strategy.Conditions;
using Player.Strategy.Conditions.StrategyConditions;
using Player.Strategy.StateInfo;

namespace Player.Strategy.States
{
    public abstract class State : ILoggable
    {
        protected List<StrategyCondition> conditions;

        protected State(StrategyInfo strategyInfo)
        {
            StrategyInfo = strategyInfo;
            conditions = new List<StrategyCondition>();
        }

        protected State()
        {
            conditions = new List<StrategyCondition>();
        }

        protected StrategyInfo StrategyInfo { get; }

        public Request GetNextMessage()
        {
            foreach (var condition in conditions)
                if (condition.CheckCondition())
                    return condition.GetNextMessage(this);

            throw new StrategyException("GetNextMessage error", this, StrategyInfo);
        }

        public State GetNextState()
        {
            foreach (var condition in conditions)
                if (condition.CheckCondition())
                    return condition.GetNextState(this);

            throw new StrategyException("GetNextState error", StrategyInfo);
        }

        public string ToLog()
        {
            return this.GetType().ToString() + StrategyInfo.ToLog();
        }
    }
}