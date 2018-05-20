using System.Collections.Generic;
using Common.BoardObjects;
using Common.Interfaces;
using Messaging.Requests;
using PlayerStateCoordinator.Common;
using PlayerStateCoordinator.Common.States;
using PlayerStateCoordinator.Common.Transitions;
using PlayerStateCoordinator.Info;
using PlayerStateCoordinator.NormalPlayer.States;

namespace PlayerStateCoordinator.NormalPlayer.Transitions
{
    internal class IsOnFieldWithPieceStrategyTransition : GameStrategyTransition
    {
        public IsOnFieldWithPieceStrategyTransition(GameStrategyInfo gameStrategyInfo) : base(gameStrategyInfo)
        {
        }

        public override State NextState => new PickupPieceStrategyState(GameStrategyInfo);

        public override IEnumerable<IMessage> Message => new List<IMessage>
        {
            new PickUpPieceRequest(GameStrategyInfo.PlayerGuid, GameStrategyInfo.GameId)
        };

        public override bool IsPossible()
        {
            var currentLocation = GameStrategyInfo.CurrentLocation;
            var taskField = GameStrategyInfo.Board[currentLocation] as TaskField;
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