using Shared;
using Shared.BoardObjects;
using Shared.GameMessages;

namespace Player.Strategy.StateTransition
{
    internal class InGoalMovingToTaskTransition : BaseTransition
    {
        public InGoalMovingToTaskTransition(Location location, CommonResources.TeamColour team, int playerId,
            Board board) : base(location, team, playerId, board)
        {
        }

        public override GameMessage ExecuteStrategy()
        {
            var taskField = board.Content[location.X, location.Y] as TaskField;

            if (taskField == null)
            {
                ChangeState = PlayerStrategy.PlayerState.InGoalMovingToTask;
                var direction = team == CommonResources.TeamColour.Red
                    ? CommonResources.MoveType.Down
                    : CommonResources.MoveType.Up;
                return new Move
                {
                    Direction = direction,
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