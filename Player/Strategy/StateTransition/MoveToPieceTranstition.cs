using System;
using System.Collections.Generic;
using System.Text;
using Shared;
using Shared.BoardObjects;
using Shared.GameMessages;

namespace Player.Strategy.StateTransition
{
    class MoveToPieceTranstition : BaseTransition
    {
        public MoveToPieceTranstition(Location location, CommonResources.TeamColour team, int playerId) : base(location, team, playerId)
        { }

        public override GameMessage ExecuteStrategy(Board board)
        {
            var taskField = board.Content[location.X, location.Y] as TaskField;
            var distanceToNearestPiece = taskField.DistanceToPiece;

            if(distanceToNearestPiece == -1)
            {
                //random move
                var r = new Random();
                int direction = r.Next() % 4;

                return new Move
                {
                    Direction = (CommonResources.MoveType)direction,
                    PlayerId = playerId
                };
            }

            if(distanceToNearestPiece == 0)
            {
                ChangeState = PlayerStrategy.PlayerState.Pick;
                return new PickUpPiece
                {
                    PlayerId = playerId
                };
            }

            ChangeState = PlayerStrategy.PlayerState.Discover;
            return new Discover
            {
                PlayerId = playerId
            };
        }
    }
}
