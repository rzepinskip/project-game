using System.Collections.Generic;
using Common.Interfaces;
using Messaging.Requests;
using PlayerStateCoordinator.Common;
using PlayerStateCoordinator.Common.States;
using PlayerStateCoordinator.Common.Transitions;
using PlayerStateCoordinator.NormalPlayer.States;

namespace PlayerStateCoordinator.NormalPlayer.Transitions
{
    public class IsInGoalWithoutPieceStrategyTransition : NormalPlayerStrategyTransition
    {
        public IsInGoalWithoutPieceStrategyTransition(NormalPlayerStrategyInfo normalPlayerStrategyInfo) : base(
            normalPlayerStrategyInfo)
        {
        }

        public override State NextState => new InGoalAreaMovingToTaskStrategyState(NormalPlayerStrategyInfo);

        public override IEnumerable<IMessage> Message
        {
            get
            {
                var currentLocation = NormalPlayerStrategyInfo.CurrentLocation;
                var direction = currentLocation.DirectionToTask(NormalPlayerStrategyInfo.Team);
                NormalPlayerStrategyInfo.TargetLocation = currentLocation.GetNewLocation(direction);
                return new List<IMessage>
                {
                    new MoveRequest(NormalPlayerStrategyInfo.PlayerGuid, NormalPlayerStrategyInfo.GameId, direction)
                };
            }
        }

        public override bool IsPossible()
        {
            var currentLocation = NormalPlayerStrategyInfo.CurrentLocation;
            return !NormalPlayerStrategyInfo.Board.IsLocationInTaskArea(currentLocation);
        }
    }
}