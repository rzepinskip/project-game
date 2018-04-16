using Messaging.Requests;
using Player.Strategy.States;

namespace Player.Strategy.Conditions
{
    public class IsInGoalWithoutPieceCondition : Condition
    {
        public IsInGoalWithoutPieceCondition(StrategyInfo strategyInfo) : base(strategyInfo)
        {
        }

        public override bool CheckCondition()
        {
            return !StrategyInfo.Board.IsLocationInTaskArea(StrategyInfo.FromLocation);
        }

        public override State GetNextState(State fromState)
        {
            return new InGoalAreaMovingToTaskState(StrategyInfo);
        }

        public override Request GetNextMessage(State fromState)
        {
            var direction = StrategyInfo.FromLocation.DirectionToTask(StrategyInfo.Team);
            StrategyInfo.ToLocation = StrategyInfo.FromLocation.GetNewLocation(direction);
            return new MoveRequest(StrategyInfo.PlayerGuid, StrategyInfo.GameId, direction);
        }
    }
}