using System;
using System.Diagnostics;
using System.Xml;

namespace Common.BoardObjects
{
    [DebuggerDisplay("[Id = {Id}, PlayerId = {PlayerId}]")]
    public class Piece
    {
        protected Piece()
        {
        }

        public Piece(int id, PieceType type, int? playerId = null) : this(id, type, playerId, DateTime.Now)
        {
        }

        public Piece(int id, PieceType type, int? playerId, DateTime timestamp)
        {
            Id = id;
            Type = type;
            Timestamp = timestamp;
            PlayerId = playerId;
        }

        public int Id { get; set; }
        public PieceType Type { get; set; }
        public int? PlayerId { get; set; }
        public DateTime Timestamp { get; set; }

        public virtual void ReadXml(XmlReader reader)
        {
            reader.MoveToContent();
            Id = int.Parse(reader.GetAttribute("id"));
            Type = reader.GetAttribute("type").GetEnumValueFor<PieceType>();
            Timestamp = DateTime.Parse(reader.GetAttribute("timestamp"));

            if (!string.IsNullOrWhiteSpace(reader.GetAttribute("playerId")))
                PlayerId = int.Parse(reader.GetAttribute("playerId"));

            var isEmptyElement = reader.IsEmptyElement;
            reader.ReadStartElement();
            if (!isEmptyElement) reader.ReadEndElement();
        }

        public virtual void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("id", Id.ToString());
            writer.WriteAttributeString("type", Type.GetXmlAttributeName());
            writer.WriteAttributeString("timestamp", Timestamp.ToString());

            if (PlayerId.HasValue)
                writer.WriteAttributeString("playerId", PlayerId.ToString());
        }
    }
}