using System.Collections.Generic;
using Common;
using Common.Interfaces;
using Messaging.Requests;
using PlayerStateCoordinator.Info;
using PlayerStateCoordinator.States;
using PlayerStateCoordinator.States.GameStrategyStates;

namespace PlayerStateCoordinator.Transitions.GameStrategyTransitions
{
    public class HasNormalPieceStrategyTransition : GameStrategyTransition
    {
        public HasNormalPieceStrategyTransition(GameStrategyInfo gameStrategyInfo) : base(gameStrategyInfo)
        {
        }

        public override State NextState => new MoveToUndiscoveredGoalStrategyState(GameStrategyInfo);

        public override IEnumerable<IMessage> Message
        {
            get
            {
                var undiscoveredGoalLocation = GameStrategyInfo.UndiscoveredGoalFields[0];
                var currentLocation = GameStrategyInfo.CurrentLocation;
                var direction = undiscoveredGoalLocation.GetDirectionFrom(currentLocation);
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
            var goalLocation = GameStrategyInfo.UndiscoveredGoalFields[0];
            var playerInfo = GameStrategyInfo.Board.Players[GameStrategyInfo.PlayerId];
            var piece = playerInfo.Piece;
            var result = false;
            if (piece != null && !currentLocation.Equals(goalLocation))
                if (piece.Type == PieceType.Normal)
                    result = true;
            return result;
        }
    }
}