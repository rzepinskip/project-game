using Common.Interfaces;
using Messaging.Requests;
using Player.Strategy.StateInfo;
using Player.Strategy.States;
using Player.Strategy.States.StrategyStates;

namespace Player.Strategy.Conditions.GameConditions
{
    public abstract class GameCondition : ICondition
    {
        public GameStateInfo GameStateInfo { get; set; }

        public GameCondition(GameStateInfo gameStateInfo)
        {
            GameStateInfo = gameStateInfo;
        }

        public abstract bool CheckCondition();

        public abstract BaseState GetNextState(BaseState fromStrategyState);
        public abstract IMessage GetNextMessage(BaseState fromStrategyState);
    }
}