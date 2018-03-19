using System.Collections.Generic;
using Shared;
using Shared.BoardObjects;
using Shared.GameMessages;
using Shared.GameMessages.PieceActions;

namespace Player.Strategy.StateTransition
{
    internal class MoveToUndiscoveredGoalTransition : BaseTransition
    {
        private readonly List<GoalField> undiscoveredGoalFields;

        public MoveToUndiscoveredGoalTransition(List<GoalField> undiscoveredGoalFields, Location location,
            CommonResources.TeamColour team, int playerId, Board board) : base(location, team, playerId, board)
        {
            this.undiscoveredGoalFields = undiscoveredGoalFields;
        }

        public override GameMessage ExecuteStrategy()
        {
            Location undiscoveredGoalLocation = undiscoveredGoalFields[0];

            if (location.Equals(undiscoveredGoalLocation))
            {
                ChangeState = PlayerStrategy.PlayerState.InGoalMovingToTask;

                undiscoveredGoalFields.RemoveAt(0);
                return new PlacePiece
                {
                    PlayerId = playerId
                };
            }

            ChangeState = PlayerStrategy.PlayerState.MoveToUndiscoveredGoal;

            var direction = undiscoveredGoalLocation.GetLocationTo(location);

            return new Move
            {
                Direction = direction,
                PlayerId = playerId
            };
        }
    }
}