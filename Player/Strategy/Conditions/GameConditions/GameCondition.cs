using Common.Interfaces;
using Player.Strategy.StateInfo;
using Player.Strategy.States;

namespace Player.Strategy.Conditions.GameConditions
{
    public abstract class GameCondition : ICondition
    {
        public GameCondition(GameStateInfo gameStateInfo)
        {
            GameStateInfo = gameStateInfo;
        }

        public GameStateInfo GameStateInfo { get; set; }

        public abstract bool CheckCondition();

        public abstract BaseState GetNextState(BaseState fromStrategyState);
        public abstract IMessage GetNextMessage(BaseState fromStrategyState);
        public abstract bool ReturnsMessage(BaseState fromStrategyState);
    }
}