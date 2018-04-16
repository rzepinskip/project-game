using System.Collections.Generic;
using Common;
using Common.BoardObjects;
using Messaging.Requests;
using Player.Strategy.States;

namespace Player.Strategy
{
    public class PlayerStrategy : IStrategy
    {
        private readonly StrategyInfo strategyInfo;
        private readonly List<GoalField> undiscoveredGoalFields = new List<GoalField>();

        public State CurrentState { get; set; }

        public PlayerStrategy(PlayerBoard board, PlayerBase player, string playerGuid, int gameId)
        {
            var teamCoefficient = player.Team == TeamColor.Blue ? 0 : 1;
            var offset = teamCoefficient * (board.TaskAreaSize + board.GoalAreaSize);

            for (var i = 0; i < board.Width; ++i)
            for (var j = offset; j < offset + board.GoalAreaSize; ++j)
                undiscoveredGoalFields.Add(board[new Location(i, j)] as GoalField);

            undiscoveredGoalFields.Shuffle();

            strategyInfo = new StrategyInfo(null, board, playerGuid, gameId, player, undiscoveredGoalFields);
            CurrentState = new InitState(strategyInfo);
        }


        public Request NextMove(Location location)
        {
            strategyInfo.FromLocation = location;

            var nextState = CurrentState.GetNextState();
            var gameMessage = CurrentState.GetNextMessage();

            CurrentState = nextState;
            return gameMessage;
        }
    }
}