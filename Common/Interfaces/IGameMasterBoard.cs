using Common.BoardObjects;

namespace Common.Interfaces
{
    public interface IGameMasterBoard : IBoard
    {
        void MarkGoalAsCompleted(GoalField goal);
    }
}