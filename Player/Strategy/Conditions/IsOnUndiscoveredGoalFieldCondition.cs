using Messaging.Requests;
using Player.Strategy.States;

namespace Player.Strategy.Conditions
{
    public class IsOnUndiscoveredGoalFieldCondition : Condition
    {
        public IsOnUndiscoveredGoalFieldCondition(StrategyInfo strategyInfo) : base(strategyInfo)
        {
        }

        public override bool CheckCondition()
        {
            var currentLocation = StrategyInfo.FromLocation;
            var goalLocation = StrategyInfo.UndiscoveredGoalFields[0];
            return currentLocation.Equals(goalLocation);
        }

        public override State GetNextState(State fromState)
        {
            return new InGoalAreaMovingToTaskState(StrategyInfo);
        }

        public override Request GetNextMessage(State fromState)
        {
            StrategyInfo.UndiscoveredGoalFields.RemoveAt(0);
            return new PlacePieceRequest(StrategyInfo.PlayerId);
        }
    }
}