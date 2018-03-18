using Player.Strategy.StateTransition.Factory;
using Shared.BoardObjects;
using Shared.GameMessages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Player.Strategy
{
    public class PlayerStrategy
    {
        private List<GoalField> undiscoveredGoalFields = new List<GoalField>();
        private StateTranstitionFactory stateTransitionFactory;
        public PlayerStrategy(Board board, Shared.CommonResources.TeamColour team, int playerId)
        {
            currentState = PlayerState.InitState;

            int teamCoefficient = team == Shared.CommonResources.TeamColour.Blue ? 0 : 1;
            int offset = teamCoefficient * (board.TaskAreaSize + board.GoalAreaSize);

            for (var i = 0; i < board.Width; ++i)
            {
                for (var j = offset; j < offset +  board.GoalAreaSize; ++j)
                    undiscoveredGoalFields.Add(board.Content[i, j] as GoalField);
            }

            this.stateTransitionFactory = new StateTranstitionFactory(board, playerId, team, undiscoveredGoalFields);
        }
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
        private PlayerState currentState;

        
        public GameMessage NextMove(Location location)
        {
            var transition = stateTransitionFactory.GetNextTranstition(currentState, location);
            GameMessage gameMessage = transition.ExecuteStrategy();
            currentState = transition.ChangeState;
            return gameMessage;

        }
    }
}
