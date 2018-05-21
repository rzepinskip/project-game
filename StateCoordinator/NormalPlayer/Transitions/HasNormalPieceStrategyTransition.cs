using System.Collections.Generic;
using Common;
using Common.Interfaces;
using Messaging.Requests;
using PlayerStateCoordinator.Common;
using PlayerStateCoordinator.Common.States;
using PlayerStateCoordinator.Common.Transitions;
using PlayerStateCoordinator.NormalPlayer.States;

namespace PlayerStateCoordinator.NormalPlayer.Transitions
{
    public class HasNormalPieceStrategyTransition : NormalPlayerStrategyTransition
    {
        public HasNormalPieceStrategyTransition(NormalPlayerStrategyInfo normalPlayerStrategyInfo) : base(normalPlayerStrategyInfo)
        {
        }

        public override State NextState => new MoveToUndiscoveredGoalStrategyState(NormalPlayerStrategyInfo);

        public override IEnumerable<IMessage> Message
        {
            get
            {
                var undiscoveredGoalLocation = NormalPlayerStrategyInfo.UndiscoveredGoalFields[0];
                var currentLocation = NormalPlayerStrategyInfo.CurrentLocation;
                var direction = undiscoveredGoalLocation.GetDirectionFrom(currentLocation);
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
            var goalLocation = NormalPlayerStrategyInfo.UndiscoveredGoalFields[0];
            var playerInfo = NormalPlayerStrategyInfo.Board.Players[NormalPlayerStrategyInfo.PlayerId];
            var piece = playerInfo.Piece;
            var result = false;
            if (piece != null && !currentLocation.Equals(goalLocation))
                if (piece.Type == PieceType.Normal)
                    result = true;
            return result;
        }
    }
}