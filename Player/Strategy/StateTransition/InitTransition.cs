using Common;
using Common.BoardObjects;
using Messaging.Requests;

namespace Player.Strategy.StateTransition
{
    internal class InitTransition : BaseTransition
    {
        public InitTransition(Location location, TeamColor team, int playerId, PlayerBoard board) : base(
            location, team, playerId, board)
        {
        }

        public override Request ExecuteStrategy()
        {
            if (board.IsLocationInTaskArea(location))
            {
                ChangeState = PlayerState.Discover;
                return new DiscoverRequest(playerId);
            }

            ChangeState = PlayerState.InGoalMovingToTask;
            var direction = team == TeamColor.Red
                ? Direction.Down
                : Direction.Up;

            return new MoveRequest(playerId, direction);
        }
    }
}