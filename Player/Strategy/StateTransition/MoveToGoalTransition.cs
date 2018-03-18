using System;
using System.Collections.Generic;
using System.Text;
using Shared;
using Shared.BoardObjects;
using Shared.GameMessages;

namespace Player.Strategy.StateTransition
{
    class MoveToGoalTransition : BaseTransition
    {
        private List<GoalField> undiscoveredGoalFields;
        public MoveToGoalTransition(List<GoalField> undiscoveredGoalFields, Location location, CommonResources.TeamColour team, int playerId) : base(location, team, playerId)
        {
            this.undiscoveredGoalFields = undiscoveredGoalFields;
        }
        public override GameMessage ExecuteStrategy(Board board)
        {
            var goalField = board.Content[location.X, location.Y] as GoalField;

            if (goalField == null)
            {
                ChangeState = PlayerStrategy.PlayerState.MoveToGoalArea;
                var direction = team == CommonResources.TeamColour.Red ? CommonResources.MoveType.Down : CommonResources.MoveType.Up;
                return new Move
                {
                    Direction = direction,
                    PlayerId = playerId
                };
            }
            else
            {
                ChangeState = PlayerStrategy.PlayerState.MoveToUndiscoveredGoal;
                Location undiscoveredGoalLocation = undiscoveredGoalFields[0];
                //CommonResources.MoveType direction = 

            }

        }
    }
}
