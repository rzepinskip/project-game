using System.Threading;
using Common.Interfaces;
using Messaging.InitializationMessages;
using Player.Strategy.StateInfo;
using Player.Strategy.States;
using Player.Strategy.States.GameStates;

namespace Player.Strategy.Conditions.GameConditions
{
    internal class IsJoinUnsuccessfulCondition : GameCondition
    {
        public IsJoinUnsuccessfulCondition(GameStateInfo gameStateInfo) : base(gameStateInfo)
        {
        }

        public override bool CheckCondition()
        {
            return !GameStateInfo.JoiningSuccessful;
        }

        public override BaseState GetNextState(BaseState fromStrategyState)
        {
            return new MatchingGameState(GameStateInfo);
        }

        public override IMessage GetNextMessage(BaseState fromStrategyState)
        {
            //Thread Sleep to avoid CS spamming
            Thread.Sleep(Constants.DefaultRequestRetryInterval);
            return new GetGamesMessage();
        }

        public override bool ReturnsMessage(BaseState fromStrategyState)
        {
            return true;
        }
    }
}