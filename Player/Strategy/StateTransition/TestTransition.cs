using Common;
using Common.BoardObjects;
using Messaging.Requests;

namespace Player.Strategy.StateTransition
{
    internal class TestTransition : BaseTransition
    {
        public TestTransition(Location location, TeamColor team, int playerId, PlayerBoard board) : base(
            location, team, playerId, board)
        {
        }

        public override Request ExecuteStrategy()
        {
            var playerInfo = board.Players[playerId];

            if (playerInfo.Piece == null)
            {
                ChangeState = PlayerState.Discover;
                return new DiscoverRequest(playerId);
            }

            ChangeState = PlayerState.MoveToGoalArea;
            var direction = team == TeamColor.Red
                ? Direction.Up
                : Direction.Down;
            return new MoveRequest(playerId, direction);

            //switch (playerInfo.Piece.Type)
            //{
            //    case PieceType.Sham:


            //    case PieceType.Normal:


            //    default:
            //        throw new Exception("STH WENT TERRIBLY WRONG");
            //}
        }
    }
}