using Common.Interfaces;
using Player.Strategy.StateInfo;
using Player.Strategy.States;

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
            throw new System.NotImplementedException();
        }

        public override IMessage GetNextMessage(BaseState fromStrategyState)
        {
            throw new System.NotImplementedException();
        }
    }
}