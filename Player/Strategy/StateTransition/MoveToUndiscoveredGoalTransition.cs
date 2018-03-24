using System.Collections.Generic;
using Common;
using Common.BoardObjects;
using Messaging.Requests;

namespace Player.Strategy.StateTransition
{
    internal class MoveToUndiscoveredGoalTransition : BaseTransition
    {
        private readonly List<GoalField> undiscoveredGoalFields;

        public MoveToUndiscoveredGoalTransition(List<GoalField> undiscoveredGoalFields, Location location,
            TeamColor team, int playerId, PlayerBoard board) : base(location, team, playerId, board)
        {
            this.undiscoveredGoalFields = undiscoveredGoalFields;
        }

        public override Request ExecuteStrategy()
        {
            Location undiscoveredGoalLocation = undiscoveredGoalFields[0];

            if (location.Equals(undiscoveredGoalLocation))
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