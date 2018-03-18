using System;
using System.Collections.Generic;
using System.Text;
using Shared;
using Shared.BoardObjects;
using Shared.GameMessages;

namespace Player.Strategy.StateTransition
{
    class InGoalMovingToTaskTransition : BaseTransition
    {
        public InGoalMovingToTaskTransition(Location location, CommonResources.TeamColour team, int playerId) : base(location, team, playerId)
        {
        }
        public override GameMessage ExecuteStrategy(Board board)
        {
            var taskField = board.Content[location.X, location.Y] as TaskField;

            if (taskField == null)
            {
                ChangeState = PlayerStrategy.PlayerState.InGoalMovingToTask;
                var direction = team == CommonResources.TeamColour.Red ? CommonResources.MoveType.Up : CommonResources.MoveType.Down;
                return new Move
                {
                    Direction = direction,
                    PlayerId = playerId
                };
            }
            else
            {
                ChangeState = PlayerStrategy.PlayerState.Discover;

                return new Discover
                {
                    PlayerId = playerId
                };
            }
        }
    }
}
