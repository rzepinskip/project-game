using System;
using System.Xml;

namespace Common.BoardObjects
{
    public class TaskField : Field
    {
        protected TaskField()
        {
        }

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

        public override void ReadXml(XmlReader reader)
        {
            base.ReadXml(reader);

            DistanceToPiece = int.Parse(reader.GetAttribute("distanceToPiece"));
            if (int.TryParse(reader.GetAttribute("pieceId"), out int pieceId))
                PieceId = pieceId;
        }

        public override void WriteXml(XmlWriter writer)
        {
            base.WriteXml(writer);

            writer.WriteAttributeString("distanceToPiece", DistanceToPiece.ToString());

            if (PieceId.HasValue)
                writer.WriteAttributeString("pieceId", PieceId.ToString());
        }
    }
}