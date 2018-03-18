using System;
using System.Collections.Generic;
using System.Text;
using Shared.BoardObjects;
using Shared.GameMessages;
using Shared;

namespace Player.Strategy.StateTransition
{
    class InitTransition : BaseTransition
    {
        public InitTransition(Location location, CommonResources.TeamColour team, int playerId, Board board) : base(location, team, playerId, board)
        {
        }

        public override GameMessage ExecuteStrategy()
        {
            if(board.IsLocationInTaskArea(location))
            {
                ChangeState = PlayerStrategy.PlayerState.Discover;
                return new Discover()
                {
                    PlayerId = playerId
                };
            }
            else
            {
                ChangeState = PlayerStrategy.PlayerState.InGoalMovingToTask;
                return new Move()
                {
                    PlayerId = playerId,
                    Direction = team == CommonResources.TeamColour.Red ? CommonResources.MoveType.Down : CommonResources.MoveType.Up
                };
            }
        }
    }
}
