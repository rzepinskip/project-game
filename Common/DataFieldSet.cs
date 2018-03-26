using Common.BoardObjects;

namespace Common
{
    public class DataFieldSet
    {
        private DataFieldSet(int playerId, TaskField[] taskFields, GoalField[] goalFields, Piece[] pieces,
            Location playerLocation)
        {
            PlayerId = playerId;
            TaskFields = taskFields;
            GoalFields = goalFields;
            Pieces = pieces;
            PlayerLocation = playerLocation;
        }

        public int PlayerId { get; }
        public TaskField[] TaskFields { get; }

        public GoalField[] GoalFields { get; }

        public Piece[] Pieces { get; }

        public Location PlayerLocation { get; }

        public static DataFieldSet CreateMoveDataSet(int playerId, TaskField[] taskFields, Piece[] pieces,
            Location playerLocation)
        {
            return new DataFieldSet(playerId, taskFields, new GoalField[0], pieces, playerLocation);
        }

        public static DataFieldSet CreateMoveDataSet(int playerId, Piece[] pieces)
        {
            return new DataFieldSet(playerId, new TaskField[0], new GoalField[0], pieces, null);
        }

        public static DataFieldSet CreateMoveDataSet(int playerId, GoalField[] goalFields)
        {
            return new DataFieldSet(playerId, new TaskField[0], goalFields, new Piece[0], null);
        }
    }
}