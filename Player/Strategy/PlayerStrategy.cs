using System;
using System.Collections.Generic;
using Common;
using Common.BoardObjects;
using Common.Interfaces;
using Player.Strategy.StateInfo;
using Player.Strategy.States;
using Player.Strategy.States.StrategyStates;

namespace Player.Strategy
{
    public class PlayerStrategy : IStrategy
    {
        private readonly StrategyInfo strategyInfo;
        private readonly List<GoalField> undiscoveredGoalFields = new List<GoalField>();


        public PlayerStrategy(PlayerBoard board, PlayerBase player, Guid playerGuid, int gameId)
        {
            var teamCoefficient = player.Team == TeamColor.Blue ? 0 : 1;
            var offset = teamCoefficient * (board.TaskAreaSize + board.GoalAreaSize);

            for (var i = 0; i < board.Width; ++i)
            for (var j = offset; j < offset + board.GoalAreaSize; ++j)
                undiscoveredGoalFields.Add(board[new Location(i, j)] as GoalField);

            undiscoveredGoalFields.Shuffle();

            strategyInfo = new StrategyInfo(null, board, playerGuid, gameId, player, undiscoveredGoalFields);
            CurrentGameState = new InitStrategyState(strategyInfo);
        }

        public BaseState CurrentGameState { get; set; }


        public IMessage NextMove()
        {
            var nextState = CurrentGameState.GetNextState();
            var gameMessage = CurrentGameState.GetNextMessage();

            CurrentGameState = nextState;
            return gameMessage;
        }

        public bool StrategyReturnsMessage()
        {
            strategyInfo.FromLocation = strategyInfo.Board.Players[strategyInfo.PlayerId].Location;
            return CurrentGameState.StateReturnsMessage();
        }
    }
}