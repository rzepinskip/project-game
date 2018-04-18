using Common.Interfaces;
using Messaging.Requests;
using Player.Strategy.StateInfo;
using Player.Strategy.States;
using Player.Strategy.States.StrategyStates;

namespace Player.Strategy.Conditions.StrategyConditions
{
    public class IsInGoalWithoutPieceStrategyCondition : StrategyCondition
    {
        public IsInGoalWithoutPieceStrategyCondition(StrategyInfo strategyInfo) : base(strategyInfo)
        {
        }

        public override bool CheckCondition()
        {
            return !StrategyInfo.Board.IsLocationInTaskArea(StrategyInfo.FromLocation);
        }

        public override BaseState GetNextState(BaseState fromStrategyState)
        {
            return new InGoalAreaMovingToTaskStrategyState(StrategyInfo);
        }

        public override IMessage GetNextMessage(BaseState fromStrategyState)
        {
            var direction = StrategyInfo.FromLocation.DirectionToTask(StrategyInfo.Team);
            StrategyInfo.ToLocation = StrategyInfo.FromLocation.GetNewLocation(direction);
            return new MoveRequest(StrategyInfo.PlayerGuid, StrategyInfo.GameId, direction);
        }
    }
}