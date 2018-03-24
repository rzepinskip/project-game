using System;
using Common;
using Common.BoardObjects;
using Messaging.Requests;

namespace Player.Strategy.StateTransition
{
    internal class RandomWalkTransition : BaseTransition
    {
        public RandomWalkTransition(Location location, TeamColor team, int playerId, PlayerBoard board) :
            base(location, team, playerId, board)
        {
        }

        public override Request ExecuteStrategy()
        {
            var taskField = board[location] as TaskField;
            var distanceToNearestPiece = taskField.DistanceToPiece;

            if (distanceToNearestPiece == -1)
            {
                //random move
                var r = new Random();
                var direction = r.Next() % 2 == 0 ? Direction.Left : Direction.Right;

                ChangeState = PlayerState.RandomWalk;
                return new MoveRequest(playerId, direction);
            }

            if (distanceToNearestPiece == 0)
            {
                ChangeState = PlayerState.Pick;
                return new PickUpPieceRequest(playerId);
            }

            ChangeState = PlayerState.Discover;
            return new DiscoverRequest(playerId);
        }
    }
}