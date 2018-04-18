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

        public StrategyState GetNextState(StrategyState fromStrategyState)
        {
            throw new System.NotImplementedException();
        }

        public Request GetNextMessage(StrategyState fromStrategyState)
        {
            throw new System.NotImplementedException();
        }
    }
}