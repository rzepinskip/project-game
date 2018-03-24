using Player.Strategy.States;
using Shared.GameMessages;

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

        public override GameMessage GetNextMessage(State fromState)
        {
            return new Move
            {
                PlayerId = StrategyInfo.PlayerId,
                Direction = StrategyInfo.FromLocation.DirectionToTask(StrategyInfo.Team)
            };
        }
    }
}