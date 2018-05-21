using System;
using System.Collections.Generic;
using Common;
using Common.BoardObjects;
using Common.Interfaces;
using Messaging.Requests;
using PlayerStateCoordinator.Common;
using PlayerStateCoordinator.Common.States;
using PlayerStateCoordinator.Common.Transitions;
using PlayerStateCoordinator.NormalPlayer.States;

namespace PlayerStateCoordinator.NormalPlayer.Transitions
{
    public class IsNoPieceAvailableStrategyTransition : NormalPlayerStrategyTransition
    {
        private readonly Random _directionGenerator;

        public IsNoPieceAvailableStrategyTransition(NormalPlayerStrategyInfo normalPlayerStrategyInfo) : base(
            normalPlayerStrategyInfo)
        {
            _directionGenerator = new Random();
        }

        public override State NextState => new MoveToPieceStrategyState(NormalPlayerStrategyInfo);

        public override IEnumerable<IMessage> Message
        {
            get
            {
                var direction = _directionGenerator.Next() % 2 == 0
                    ? Direction.Left
                    : Direction.Right;
                var currentLocation = NormalPlayerStrategyInfo.CurrentLocation;
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
            var taskField = NormalPlayerStrategyInfo.Board[currentLocation] as TaskField;
            var result = false;
            if (taskField != null)
            {
                var distanceToNearestPiece = taskField.DistanceToPiece;
                if (distanceToNearestPiece == -1)
                    result = true;
            }

            return result;
        }
    }
}