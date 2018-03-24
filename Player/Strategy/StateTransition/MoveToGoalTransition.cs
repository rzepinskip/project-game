using System.Collections.Generic;
using Common;
using Common.BoardObjects;
using Messaging.Requests;

namespace Player.Strategy.StateTransition
{
    internal class MoveToGoalTransition : BaseTransition
    {
        private readonly List<GoalField> undiscoveredGoalFields;

        public MoveToGoalTransition(List<GoalField> undiscoveredGoalFields, Location location,
            TeamColor team, int playerId, PlayerBoard board) : base(location, team, playerId, board)
        {
            this.undiscoveredGoalFields = undiscoveredGoalFields;
        }

        public override Request ExecuteStrategy()
        {
            var goalField = board[location] as GoalField;

            if (goalField == null)
            {
                ChangeState = PlayerState.MoveToGoalArea;
                var direction = team == TeamColor.Red
                    ? Direction.Up
                    : Direction.Down;
                return new MoveRequest(playerId, direction);
            }
            else
            {
                Location undiscoveredGoalLocation = undiscoveredGoalFields[0];
                if (location.Equals(undiscoveredGoalFields[0]))
                {
                    ChangeState = PlayerState.InGoalMovingToTask;
                    undiscoveredGoalFields.RemoveAt(0);
                    return new PlacePieceRequest(playerId);
                }

                ChangeState = PlayerState.MoveToUndiscoveredGoal;
                var direction = undiscoveredGoalLocation.GetLocationTo(location);

                return new MoveRequest(playerId, direction);
            }
        }
    }
}