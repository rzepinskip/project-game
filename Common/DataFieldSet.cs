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

        public static DataFieldSet Create(int playerId, TaskField[] taskFields, Piece[] pieces,
            Location playerLocation)
        {
            if (taskFields.Length == 0)
                taskFields = null;

            if (pieces.Length == 0)
                pieces = null;

            return new DataFieldSet(playerId, taskFields, null, pieces, playerLocation);
        }

        public static DataFieldSet Create(int playerId, Piece[] pieces)
        {
            return new DataFieldSet(playerId, null, null, pieces, null);
        }

        public static DataFieldSet Create(int playerId, GoalField[] goalFields)
        {
            return new DataFieldSet(playerId,null, goalFields, null, null);
        }
    }
}