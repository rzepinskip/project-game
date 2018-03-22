using System.Collections.Generic;
using Common;
using Common.BoardObjects;
using Messaging.Requests;
using Player.Strategy.StateTransition.Factory;

namespace Player.Strategy
{
    public enum PlayerState
    {
        InitState,
        InGoalMovingToTask,
        MoveToPiece,
        Discover,
        RandomWalk,
        Pick,
        Test,
        MoveToGoalArea,
        MoveToUndiscoveredGoal,
        Place
    }

    public class PlayerStrategy
    {
        private readonly StateTranstitionFactory stateTransitionFactory;
        private readonly List<GoalField> undiscoveredGoalFields = new List<GoalField>();

        private PlayerState currentState;

        public PlayerStrategy(PlayerBoard board, TeamColor team, int playerId)
        {
            currentState = PlayerState.InitState;

            var teamCoefficient = team == TeamColor.Blue ? 0 : 1;
            var offset = teamCoefficient * (board.TaskAreaSize + board.GoalAreaSize);

            for (var i = 0; i < board.Width; ++i)
            for (var j = offset; j < offset + board.GoalAreaSize; ++j)
                undiscoveredGoalFields.Add(board[new Location(i, j)] as GoalField);

            stateTransitionFactory = new StateTranstitionFactory(board, playerId, team, undiscoveredGoalFields);
        }


        public Request NextMove(Location location)
        {
            var transition = stateTransitionFactory.GetNextTranstition(currentState, location);
            var gameMessage = transition.ExecuteStrategy();
            currentState = transition.ChangeState;
            return gameMessage;
        }
    }
}