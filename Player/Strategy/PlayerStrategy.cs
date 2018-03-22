using System.Collections.Generic;
using Player.Strategy.StateTransition.Factory;
using Shared;
using Shared.BoardObjects;
using Shared.GameMessages;

namespace Player.Strategy
{
    public class PlayerStrategy
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

        private readonly StateTranstitionFactory stateTransitionFactory;
        private readonly List<GoalField> undiscoveredGoalFields = new List<GoalField>();

        private PlayerState currentState;

        public PlayerStrategy(Board board, CommonResources.TeamColour team, int playerId)
        {
            currentState = PlayerState.InitState;

            var teamCoefficient = team == CommonResources.TeamColour.Blue ? 0 : 1;
            var offset = teamCoefficient * (board.TaskAreaSize + board.GoalAreaSize);

            for (var i = 0; i < board.Width; ++i)
            for (var j = offset; j < offset + board.GoalAreaSize; ++j)
                undiscoveredGoalFields.Add(board.Content[i, j] as GoalField);

            stateTransitionFactory = new StateTranstitionFactory(board, playerId, team, undiscoveredGoalFields);
        }


        public GameMessage NextMove(Location location)
        {
            var transition = stateTransitionFactory.GetNextTranstition(currentState, location);
            var gameMessage = transition.ExecuteStrategy();
            currentState = transition.ChangeState;
            return gameMessage;
        }
    }
}