using System;
using System.Collections.Generic;
using System.Text;
using Common.Interfaces;
using Player.Strategy.StateInfo;
using Player.Strategy.States;
using Player.Strategy.States.GameStates;

namespace Player.Strategy.Conditions.GameConditions
{
    class IsJoinUnsuccessfulCondition : GameCondition
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
            return new GetGamesState(GameStateInfo);
        }

        public override IMessage GetNextMessage(BaseState fromStrategyState)
        {
            
            throw new System.NotImplementedException();
        }
    }
}
