using System;
using Common.Interfaces;
using Player.Strategy.StateInfo;
using Player.Strategy.States;
using Player.Strategy.States.GameStates;

namespace Player.Strategy.Conditions.GameConditions
{
    public class HasGameEndedCondition : GameCondition
    {
        public HasGameEndedCondition(GameStateInfo gameStateInfo)
            : base(gameStateInfo)
        {
        }

        public override bool CheckCondition()
        {
            return !GameStateInfo.IsRunning;
        }

        public override BaseState GetNextState(BaseState fromStrategyState)
        {
            return new GameStartedState(GameStateInfo);
        }

        public override IMessage GetNextMessage(BaseState fromStrategyStates)
        {
            throw new NotImplementedException();
        }

        public override bool ReturnsMessage(BaseState fromStrategyState)
        {
            return false;
        }
    }
}