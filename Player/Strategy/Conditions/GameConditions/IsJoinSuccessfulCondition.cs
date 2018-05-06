using System;
using Common.Interfaces;
using Player.Strategy.StateInfo;
using Player.Strategy.States;
using Player.Strategy.States.GameStates;

namespace Player.Strategy.Conditions.GameConditions
{
    public class IsJoinSuccessfulCondition : GameCondition
    {
        public IsJoinSuccessfulCondition(GameStateInfo gameStateInfo) : base(gameStateInfo)
        {
        }

        public override bool CheckCondition()
        {
            return GameStateInfo.JoiningSuccessful;
        }

        public override BaseState GetNextState(BaseState fromStrategyState)
        {
            return new GameStartedState(GameStateInfo);
        }

        public override IMessage GetNextMessage(BaseState fromStrategyState)
        {
            //Wainting for another response, not sending message
            throw new NotImplementedException();
        }

        public override bool ReturnsMessage(BaseState fromStrategyState)
        {
            return false;
        }
    }
}