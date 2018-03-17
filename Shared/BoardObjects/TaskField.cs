using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Shared.BoardObjects
{
    public class TaskField : Field
    {
        public TaskField(int distanceToPiece, int? pieceId, int? playerId, DateTime date, int x, int y) : base(playerId, date, x,y)
        {
            DistanceToPiece = distanceToPiece;
            PieceId = pieceId;
        }

        public int DistanceToPiece { get; set; }
        public int? PieceId { get; set; }

        public int GetManhattanDistance(Location l)
        {
            return Math.Abs(X - l.X) + Math.Abs(Y - l.Y);
        }
    }
}
