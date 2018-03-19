using Shared;
using Shared.BoardObjects;
using Shared.GameMessages;

namespace Player.Strategy.StateTransition
{
    internal class InitTransition : BaseTransition
    {
        public InitTransition(Location location, CommonResources.TeamColour team, int playerId, Board board) : base(
            location, team, playerId, board)
        {
        }

        public override GameMessage ExecuteStrategy()
        {
            if (board.IsLocationInTaskArea(location))
            {
                ChangeState = PlayerStrategy.PlayerState.Discover;
                return new Discover
                {
                    PlayerId = playerId
                };
            }

            ChangeState = PlayerStrategy.PlayerState.InGoalMovingToTask;
            return new Move
            {
                PlayerId = playerId,
                Direction = team == CommonResources.TeamColour.Red
                    ? CommonResources.MoveType.Down
                    : CommonResources.MoveType.Up
            };
        }
    }
}