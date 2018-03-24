using System;
using System.Collections.Generic;
using Player.Strategy.Conditions;
using Shared.GameMessages;

namespace Player.Strategy.States
{
    public abstract class State
    {
        protected List<Condition> conditions;

        protected State(StrategyInfo strategyInfo)
        {
            StrategyInfo = strategyInfo;
            conditions = new List<Condition>();
        }

        protected State()
        {
            conditions = new List<Condition>();
        }

        protected StrategyInfo StrategyInfo { get; }

        public GameMessage GetNextMessage()
        {
            foreach (var condition in conditions)
                if (condition.CheckCondition())
                    return condition.GetNextMessage(this);

            throw new StrategyException("GetNextMessage error", this, this.StrategyInfo);
        }

        public State GetNextState()
        {
            foreach (var condition in conditions)
                if (condition.CheckCondition())
                    return condition.GetNextState(this);

            throw new StrategyException("GetNextState error", this.StrategyInfo);
        }
    }
}