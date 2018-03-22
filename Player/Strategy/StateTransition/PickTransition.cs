using Common;
using Common.BoardObjects;
using Messaging.Requests;

namespace Player.Strategy.StateTransition
{
    internal class PickTransition : BaseTransition
    {
        public PickTransition(Location location, TeamColor team, int playerId, PlayerBoard board) : base(
            location, team, playerId, board)
        {
        }

        public override Request ExecuteStrategy()
        {
            ChangeState = PlayerState.Test;
            return new TestPieceRequest(playerId);
        }
    }
}