using Common.BoardObjects;

namespace Common.Interfaces
{
    public interface IPlayerBoard
    {
        void HandlePlayerLocation(int playerId, Location playerLocation);
        void HandlePiece(int playerId, Piece piece);
        void HandleTaskField(int playerId, TaskField taskField);
        void HandleGoalField(int playerId, GoalField goalField);
    }
}