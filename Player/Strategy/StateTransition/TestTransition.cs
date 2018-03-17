using System;
using System.Collections.Generic;
using System.Text;
using Shared;
using Shared.BoardObjects;
using Shared.GameMessages;
using Shared.GameMessages.PieceActions;

namespace Player.Strategy.StateTransition
{
    class TestTransition : BaseTransition
    {
        public TestTransition(Location location, CommonResources.TeamColour team, int playerId) : base(location, team, playerId)
        { }

        public override GameMessage ExecuteStrategy(Board board)
        {
            var playerInfo = board.Players[playerId];

            switch (playerInfo.Piece.Type)
            {
                case CommonResources.PieceType.Sham:
                    ChangeState = PlayerStrategy.PlayerState.Discover;
                    return new Discover
                    {
                        PlayerId = playerId
                    };

                case CommonResources.PieceType.Normal:
                    ChangeState = PlayerStrategy.PlayerState.MoveToGoal;
                    var direction = team == CommonResources.TeamColour.Red ? CommonResources.MoveType.Down : CommonResources.MoveType.Up;
                    return new Move
                    {
                        Direction = direction,
                        PlayerId = playerId
                    };

                default:
                    throw new Exception("STH WENT TERRIBLY WRONG");
            }
        }
    }
}
