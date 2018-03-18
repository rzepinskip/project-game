using System;
using System.Collections.Generic;
using System.Text;
using Shared;
using Shared.BoardObjects;
using Shared.GameMessages;
using Shared.GameMessages.PieceActions;
using System.Diagnostics;

namespace Player.Strategy.StateTransition
{
    class MoveToUndiscoveredGoalTransition : BaseTransition
    {
        private List<GoalField> undiscoveredGoalFields;
        public MoveToUndiscoveredGoalTransition(List<GoalField> undiscoveredGoalFields, Location location,
            CommonResources.TeamColour team, int playerId, Board board) : base(location, team, playerId, board)
        {
            this.undiscoveredGoalFields = undiscoveredGoalFields;
        }

        public override GameMessage ExecuteStrategy()
        {
            Location undiscoveredGoalLocation = undiscoveredGoalFields[0];

            if(undiscoveredGoalLocation == location)
            {
                ChangeState = PlayerStrategy.PlayerState.InGoalMovingToTask;

                undiscoveredGoalFields.RemoveAt(0);
                Debug.WriteLine("Placed piece");
                return new PlacePiece
                {
                    PlayerId = playerId
                };
            }

            CommonResources.MoveType direction = undiscoveredGoalLocation.GetLocationTo(location);

            return new Move
            {
                Direction = direction,
                PlayerId = playerId
            };
        }
    }
}
