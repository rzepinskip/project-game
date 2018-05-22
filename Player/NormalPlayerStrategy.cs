using System;
using System.Collections.Generic;
using Common;
using Common.BoardObjects;
using PlayerStateCoordinator.Common.States;
using PlayerStateCoordinator.GamePlay.NormalPlayer;
using PlayerStateCoordinator.GamePlay.NormalPlayer.States;

namespace Player
{
    public class NormalPlayerStrategy : Strategy
    {
        protected NormalPlayerStrategyState BeginningState;
        protected NormalPlayerStrategyInfo NormalPlayerStrategyInfo;

        public NormalPlayerStrategy(PlayerBase player, BoardBase board, Guid playerGuid, int gameId) : base(player,
            board, playerGuid, gameId)
        {
            var teamCoefficient = player.Team == TeamColor.Blue ? 0 : 1;
            var offset = teamCoefficient * (board.TaskAreaSize + board.GoalAreaSize);
            var undiscoveredGoalFields = new List<GoalField>();

            for (var i = 0; i < board.Width; ++i)
            for (var j = offset; j < offset + board.GoalAreaSize; ++j)
                undiscoveredGoalFields.Add(board[new Location(i, j)] as GoalField);

            undiscoveredGoalFields.Shuffle();

            NormalPlayerStrategyInfo = new NormalPlayerStrategyInfo(board, player.Id, playerGuid, gameId, player.Team,
                undiscoveredGoalFields, new Location(board.Width / 2, board.GoalAreaSize + 1));
            BeginningState = new InitStrategyState(NormalPlayerStrategyInfo);
        }

        public override State GetBeginningState()
        {
            return BeginningState;
        }
    }
}