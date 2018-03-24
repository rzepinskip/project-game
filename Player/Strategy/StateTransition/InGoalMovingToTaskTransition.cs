using Common;
using Common.BoardObjects;
using Messaging.Requests;

namespace Player.Strategy.StateTransition
{
    internal class InGoalMovingToTaskTransition : BaseTransition
    {
        public InGoalMovingToTaskTransition(Location location, TeamColor team, int playerId,
            PlayerBoard board) : base(location, team, playerId, board)
        {
        }

        public override Request ExecuteStrategy()
        {
            var taskField = board[location] as TaskField;

            if (taskField == null)
            {
                ChangeState = PlayerState.InGoalMovingToTask;
                var direction = team == TeamColor.Red
                    ? Direction.Down
                    : Direction.Up;

                return new MoveRequest(playerId, direction);
            }

            ChangeState = PlayerState.Discover;

            return new DiscoverRequest(playerId);
        }
    }
}