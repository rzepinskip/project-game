using Common.Interfaces;
using Messaging.InitializationMessages;
using Player.Strategy.StateInfo;
using Player.Strategy.States;
using Player.Strategy.States.GameStates;

namespace Player.Strategy.Conditions.GameConditions
{
    public class GetGamesCondition : GameCondition
    {
        public GetGamesCondition(GameStateInfo gameStateInfo) : base(gameStateInfo)
        {
        }

        public override bool CheckCondition()
        {
            return true;
        }

        public override IMessage GetNextMessage(BaseState fromStrategyState)
        {
            return new GetGamesMessage();
        }

        public override BaseState GetNextState(BaseState fromStrategyState)
        {
            return new MatchingGameState(GameStateInfo);
        }

        public override bool ReturnsMessage(BaseState fromStrategyState)
        {
            return true;
        }
    }
}