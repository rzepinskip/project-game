using System.Collections.Generic;
using Common.BoardObjects;
using Common.Interfaces;
using Messaging.Requests;
using PlayerStateCoordinator.Common.States;
using PlayerStateCoordinator.GamePlay.NormalPlayer.States;

namespace PlayerStateCoordinator.GamePlay.NormalPlayer.Transitions
{
    internal class IsOnFieldWithPieceStrategyTransition : NormalPlayerStrategyTransition
    {
        public IsOnFieldWithPieceStrategyTransition(NormalPlayerStrategyInfo normalPlayerStrategyInfo) : base(
            normalPlayerStrategyInfo)
        {
        }

        public override State NextState => new PickupPieceStrategyState(NormalPlayerStrategyInfo);

        public override IEnumerable<IMessage> Message => new List<IMessage>
        {
            new PickUpPieceRequest(NormalPlayerStrategyInfo.PlayerGuid, NormalPlayerStrategyInfo.GameId)
        };

        public override bool IsPossible()
        {
            var currentLocation = NormalPlayerStrategyInfo.CurrentLocation;
            var taskField = NormalPlayerStrategyInfo.Board[currentLocation] as TaskField;
            var result = false;
            if (taskField != null)
            {
                var distanceToNearestPiece = taskField.DistanceToPiece;
                if (distanceToNearestPiece == 0)
                    result = true;
            }

            return result;
        }
    }
}