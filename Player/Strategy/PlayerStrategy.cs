using Shared.BoardObjects;
using Shared.GameMessages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Player.Strategy
{
    public class PlayerStrategy
    {
        private List<GoalField> undiscoveredGoalFields;
        public PlayerStrategy(Board board, Shared.CommonResources.TeamColour team)
        {
            currentState = PlayerState.InitState;

            int teamCoefficient = team == Shared.CommonResources.TeamColour.Blue ? 0 : 1;
            int offset = teamCoefficient * (board.TaskAreaSize + board.GoalAreaSize);

            for (var i = 0; i < board.Width; ++i)
            {
                for (var j = offset; j < offset +  board.GoalAreaSize; ++j)
                    undiscoveredGoalFields.Add(board.Content[i, j] as GoalField);
            }
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
            //etc
        }
        private PlayerState currentState;

        
        public void ChangeState(Board currentBoard)
        {

        }
        public GameMessage NextMove()
        {
            
            switch (currentState)
            {
                //case PlayerState.InitState:
            }
            
            throw new NotImplementedException();

        }
    }
}
