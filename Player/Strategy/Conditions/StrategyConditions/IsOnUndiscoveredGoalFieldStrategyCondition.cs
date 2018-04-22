using Common.Interfaces;
using Messaging.Requests;
using Player.Strategy.StateInfo;
using Player.Strategy.States;
using Player.Strategy.States.StrategyStates;

namespace Player.Strategy.Conditions.StrategyConditions
{
    public class IsOnUndiscoveredGoalFieldStrategyCondition : StrategyCondition
    {
        public IsOnUndiscoveredGoalFieldStrategyCondition(StrategyInfo strategyInfo) : base(strategyInfo)
        {
        }

        public override bool CheckCondition()
        {
            var currentLocation = StrategyInfo.FromLocation;
            var goalLocation = StrategyInfo.UndiscoveredGoalFields[0];
            return currentLocation.Equals(goalLocation);
        }

        public override BaseState GetNextState(BaseState fromStrategyState)
        {
            return new InGoalAreaMovingToTaskStrategyState(StrategyInfo);
        }

        public override IMessage GetNextMessage(BaseState fromStrategyState)
        {
            StrategyInfo.UndiscoveredGoalFields.RemoveAt(0);
            return new PlacePieceRequest(StrategyInfo.PlayerGuid, StrategyInfo.GameId);
        }

        public override bool ReturnsMessage(BaseState fromStrategyState)
        {
            return true;
        }
    }
}