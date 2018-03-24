using System;

namespace Common.BoardObjects
{
    public class TaskField : Field
    {
        public TaskField(Location location, int distanceToPiece = -1, int? pieceId = null, int? playerId = null) : this(
            location, distanceToPiece, pieceId, playerId, DateTime.Now)
        {
        }

        public TaskField(Location location, int distanceToPiece, int? pieceId, int? playerId, DateTime date) : base(
            location, playerId, date)
        {
            DistanceToPiece = distanceToPiece;
            PieceId = pieceId;
        }

        public int DistanceToPiece { get; set; }
        public int? PieceId { get; set; }

        public int ManhattanDistanceTo(Location location)
        {
            return Math.Abs(X - location.X) + Math.Abs(Y - location.Y);
        }
    }
}