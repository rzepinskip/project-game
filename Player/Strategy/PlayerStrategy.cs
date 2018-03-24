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

        private State currentState;

        public PlayerStrategy(PlayerBoard board, TeamColor team, int playerId)
        {
            var teamCoefficient = team == TeamColor.Blue ? 0 : 1;
            var offset = teamCoefficient * (board.TaskAreaSize + board.GoalAreaSize);

            for (var i = 0; i < board.Width; ++i)
            for (var j = offset; j < offset + board.GoalAreaSize; ++j)
                undiscoveredGoalFields.Add(board[new Location(i, j)] as GoalField);

            undiscoveredGoalFields.Shuffle();

            strategyInfo = new StrategyInfo(null, board, playerId, team, undiscoveredGoalFields);
            currentState = new InitState(strategyInfo);
        }


        public Request NextMove(Location location)
        {
            strategyInfo.FromLocation = location;

            var nextState = currentState.GetNextState();
            var gameMessage = currentState.GetNextMessage();

            currentState = nextState;
            return gameMessage;
        }
    }
}