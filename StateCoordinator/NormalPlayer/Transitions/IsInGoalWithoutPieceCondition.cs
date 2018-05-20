using System.Collections.Generic;
using Common.Interfaces;
using Messaging.Requests;
using PlayerStateCoordinator.Common;
using PlayerStateCoordinator.Common.States;
using PlayerStateCoordinator.Common.Transitions;
using PlayerStateCoordinator.Info;
using PlayerStateCoordinator.NormalPlayer.States;

namespace PlayerStateCoordinator.NormalPlayer.Transitions
{
    public class IsInGoalWithoutPieceStrategyTransition : GameStrategyTransition
    {
        public IsInGoalWithoutPieceStrategyTransition(GameStrategyInfo gameStrategyInfo) : base(gameStrategyInfo)
        {
        }

        public override State NextState => new InGoalAreaMovingToTaskStrategyState(GameStrategyInfo);

        public override IEnumerable<IMessage> Message
        {
            get
            {
                var currentLocation = GameStrategyInfo.CurrentLocation;
                var direction = currentLocation.DirectionToTask(GameStrategyInfo.Team);
                GameStrategyInfo.TargetLocation = currentLocation.GetNewLocation(direction);
                return new List<IMessage>
                {
                    new MoveRequest(GameStrategyInfo.PlayerGuid, GameStrategyInfo.GameId, direction)
                };
            }
        }

        public override bool IsPossible()
        {
            var currentLocation = GameStrategyInfo.CurrentLocation;
            return !GameStrategyInfo.Board.IsLocationInTaskArea(currentLocation);
        }
    }
}