using Common.Interfaces;
using Player.Strategy.StateInfo;
using Player.Strategy.States;

namespace Player.Strategy.Conditions.GameConditions
{
    internal class HasGameStartedCondition : GameCondition
    {
        public HasGameStartedCondition(GameStateInfo gameStateInfo)
            : base(gameStateInfo)
        {
        }

        public override bool CheckCondition()
        {
            return GameStateInfo.IsRunning;
        }

        public override BaseState GetNextState(BaseState fromStrategyState)
        {
            return fromStrategyState;
        }

        public override IMessage GetNextMessage(BaseState fromStrategyState)
        {
            return GameStateInfo.PlayerStrategy.NextMove();
        }

        public override bool ReturnsMessage(BaseState fromStrategyState)
        {
            return GameStateInfo.PlayerStrategy.StrategyReturnsMessage();
        }
    }
}