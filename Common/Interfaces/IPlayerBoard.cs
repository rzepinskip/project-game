using System.Collections.Generic;
using Common.BoardObjects;

namespace Common.Interfaces
{
    public interface IPlayerBoard : IBoard
    {
        void HandlePlayerLocation(int playerId, Location playerLocation);
        void HandlePiece(int playerId, Piece piece);
        void HandleTaskField(TaskField taskField);
        void HandleGoalField(GoalField goalField);
        void HandleGoalFieldAfterPlace(int playerId, GoalField goalField);
    }
}