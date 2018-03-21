using Common.BoardObjects;

namespace Common
{
    public interface IPlayerBoard
    {
        void HandlePlayerLocation(Location playerLocation);
        void HandlePiece(Piece piece);
        void HandleTaskField(TaskField taskField);
        void HandleGoalField(GoalField goalField);
    }
}