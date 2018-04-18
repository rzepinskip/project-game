using Common.Interfaces;
using Messaging.Requests;
using Player.Strategy.States;
using Player.Strategy.States.StrategyStates;

namespace Player.Strategy.Conditions.GameConditions
{
    public abstract class GameCondition : ICondition
    {
        public bool CheckCondition()
        {
            throw new System.NotImplementedException();
        }

        public BaseState GetNextState(BaseState fromStrategyState)
        {
            throw new System.NotImplementedException();
        }

        public IMessage GetNextMessage(BaseState fromStrategyState)
        {
            throw new System.NotImplementedException();
        }
    }
}