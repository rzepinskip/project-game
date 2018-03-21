using Common;
using System;
using Common.BoardObjects;

namespace Player
{
    public class PlayerBoard : BoardBase, IPlayerBoard
    {
        protected PlayerBoard(int boardWidth, int taskAreaSize, int goalAreaSize) : base(boardWidth, taskAreaSize, goalAreaSize)
        {
        }

        public void HandlePlayerLocation(Location playerLocation)
        {
            throw new NotImplementedException();
        }

        public void HandlePiece(Piece piece)
        {
            throw new NotImplementedException();
        }

        public void HandleTaskField(TaskField taskField)
        {
            throw new NotImplementedException();
        }

        public void HandleGoalField(GoalField goalField)
        {
            throw new NotImplementedException();
        }
    }
}
