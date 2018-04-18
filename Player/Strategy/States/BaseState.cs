using System;
using System.Collections.Generic;
using System.Text;
using Common.Interfaces;
using Player.Strategy.Conditions;
using Player.Strategy.StateInfo;
using Player.Strategy;

namespace Player.Strategy.States
{
    public abstract class BaseState:IExceptionContentProvider

    {
    protected List<ICondition> transitionConditions;
    protected BaseInfo stateInfo;

        public IMessage GetNextMessage()
        {
            foreach (var condition in transitionConditions)
                if (condition.CheckCondition())
                    return condition.GetNextMessage(this);
            throw new StrategyException("GetNextMessage error", this, stateInfo);
        }
        public BaseState GetNextState()
        {
            foreach (var condition in transitionConditions)
                if (condition.CheckCondition())
                    return condition.GetNextState(this);

            throw new StrategyException("GetNextState error", stateInfo);
        }

        public string GetExceptionInfo()
        {
            return ToString();
        }
    }
}
