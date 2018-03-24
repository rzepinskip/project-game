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
            return new MoveRequest(StrategyInfo.PlayerId, StrategyInfo.FromLocation.DirectionToTask(StrategyInfo.Team));
        }
    }
}