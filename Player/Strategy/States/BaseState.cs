using System.Collections.Generic;
using Common.Interfaces;
using Player.Strategy.Conditions;
using Player.Strategy.StateInfo;

namespace Player.Strategy.States
{
    public abstract class BaseState : IExceptionContentProvider

    {
        protected BaseInfo stateInfo;
        protected List<ICondition> transitionConditions;


        public string GetExceptionInfo()
        {
            return ToString();
        }

        public IMessage GetNextMessage()
        {
            foreach (var condition in transitionConditions)
                if (condition.CheckCondition())
                    return condition.GetNextMessage(this);
            throw new StrategyException("GetNextMessage error", stateInfo, this);
        }

        public BaseState GetNextState()
        {
            foreach (var condition in transitionConditions)
                if (condition.CheckCondition())
                    return condition.GetNextState(this);

            throw new StrategyException("GetNextState error", stateInfo, this);
        }

        public bool StateReturnsMessage()
        {
            foreach (var condition in transitionConditions)
                if (condition.CheckCondition())
                    return condition.ReturnsMessage(this);

            throw new StrategyException("StateReturnsMessage error", stateInfo, this);
        }
    }
}