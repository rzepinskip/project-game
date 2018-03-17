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
        public MoveToGoalTransition(Location location, CommonResources.TeamColour team, int playerId) : base(location, team, playerId)
        { }
        public override GameMessage ExecuteStrategy(Board board)
        {
            var goalField = board.Content[location.X, location.Y] as GoalField;

            if (goalField == null)
                ChangeState = PlayerStrategy.PlayerState.InGoalMovingToTask;
            else
                ChangeState = PlayerStrategy.PlayerState.MoveToGoal;

            var direction = team == CommonResources.TeamColour.Red ? CommonResources.MoveType.Down : CommonResources.MoveType.Up;
            return new Move
            {
                Direction = direction,
                PlayerId = playerId
            };
        }
    }
}
