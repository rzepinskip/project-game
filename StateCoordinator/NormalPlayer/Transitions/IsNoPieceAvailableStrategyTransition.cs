using System;
using System.Collections.Generic;
using Common;
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
    public class IsNoPieceAvailableStrategyTransition : GameStrategyTransition
    {
        private readonly Random _directionGenerator;

        public IsNoPieceAvailableStrategyTransition(GameStrategyInfo gameStrategyInfo) : base(gameStrategyInfo)
        {
            _directionGenerator = new Random();
        }

        public override State NextState => new MoveToPieceStrategyState(GameStrategyInfo);

        public override IEnumerable<IMessage> Message
        {
            get
            {
                var direction = _directionGenerator.Next() % 2 == 0
                    ? Direction.Left
                    : Direction.Right;
                var currentLocation = GameStrategyInfo.CurrentLocation;
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
            var taskField = GameStrategyInfo.Board[currentLocation] as TaskField;
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