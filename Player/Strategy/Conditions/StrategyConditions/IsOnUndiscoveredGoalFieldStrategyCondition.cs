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

        public override StrategyState GetNextState(StrategyState fromStrategyState)
        {
            return new InGoalAreaMovingToTaskStrategyState(StrategyInfo);
        }

        public override Request GetNextMessage(StrategyState fromStrategyState)
        {
            StrategyInfo.UndiscoveredGoalFields.RemoveAt(0);
            return new PlacePieceRequest(StrategyInfo.PlayerGuid, StrategyInfo.GameId);
        }
    }
}