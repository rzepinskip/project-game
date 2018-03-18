using System;
using System.Collections.Generic;
using System.Text;
using Shared;
using Shared.BoardObjects;
using Shared.GameMessages;

namespace Player.Strategy.StateTransition
{
    class RandomWalkTransition : BaseTransition
    {
        public RandomWalkTransition(Location location, CommonResources.TeamColour team, int playerId, Board board) : base(location, team, playerId, board)
        {
        }

        public override GameMessage ExecuteStrategy()
        {
            var taskField = board.Content[location.X, location.Y] as TaskField;
            var distanceToNearestPiece = taskField.DistanceToPiece;

            if(distanceToNearestPiece == -1)
            {
                //random move
                var r = new Random();
                CommonResources.MoveType direction = r.Next() % 2 == 0 ? CommonResources.MoveType.Left : CommonResources.MoveType.Right;

                ChangeState = PlayerStrategy.PlayerState.RandomWalk;
                return new Move
                {
                    Direction = direction,
                    PlayerId = playerId
                };
            }
            else
            {

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
}
