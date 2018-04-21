using System.Linq;
using Common.Interfaces;
using Messaging.InitialisationMessages;
using Player.Strategy.StateInfo;
using Player.Strategy.States;
using Player.Strategy.States.GameStates;

namespace Player.Strategy.Conditions.GameConditions
{
    public class HasMatchingGame : GameCondition
    {

        public HasMatchingGame(GameStateInfo gameStateInfo) : base(gameStateInfo)
        {
        }

        public override bool CheckCondition()
        {
            return GameStateInfo.GameInfo.Any(x => x.GameName == GameStateInfo.GameName);
        }

        public override BaseState GetNextState(BaseState fromStrategyState)
        {
            return new AwaitingJoinResponseState(GameStateInfo);
        }

        public override IMessage GetNextMessage(BaseState fromStrategyState)
        {
            return new JoinGameMessage(GameStateInfo.GameName, GameStateInfo.Role, GameStateInfo.Color);
        }
    }
}