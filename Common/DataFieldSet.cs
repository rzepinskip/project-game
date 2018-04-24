using Common.BoardObjects;

namespace Common
{
    public class DataFieldSet
    {
        private DataFieldSet(int playerId, Location playerLocation, TaskField[] taskFields, GoalField[] goalFields,
            Piece[] pieces)
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

        public static DataFieldSet Create(int playerId, Location playerLocation, TaskField[] taskFields, Piece[] pieces)
        {
            return new DataFieldSet(playerId, playerLocation, taskFields, new GoalField[0], pieces);
        }

        public static DataFieldSet Create(int playerId, TaskField[] taskFields, Piece[] pieces)
        {
            return new DataFieldSet(playerId, null, taskFields, new GoalField[0], pieces);
        }

        public static DataFieldSet Create(int playerId, Piece[] pieces)
        {
            return new DataFieldSet(playerId, null, new TaskField[0], new GoalField[0], pieces);
        }

        public static DataFieldSet Create(int playerId, GoalField[] goalFields)
        {
            return new DataFieldSet(playerId, null, new TaskField[0], goalFields, new Piece[0]);
        }
    }
}