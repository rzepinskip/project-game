using Common.BoardObjects;

namespace Common.Interfaces
{
    public interface IPlayerBoard : IBoard
    {
        void HandlePlayerLocation(int playerId, Location playerLocation);
        void HandlePiece(int playerId, Piece piece);
        void HandleTaskField(int playerId, TaskField taskField);
        void HandleGoalField(int playerId, GoalField goalField);
        void HandleGoalFieldAfterPlace(int playerId, GoalField goalField);
    }
}