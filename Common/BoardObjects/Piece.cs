using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Common.BoardObjects
{
    [DebuggerDisplay("[Id = {Id}, PlayerId = {PlayerId}]")]
    public class Piece : IXmlSerializable, IEquatable<Piece>
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

        public bool Equals(Piece other)
        {
            return other != null &&
                   Id == other.Id &&
                   Type == other.Type &&
                   EqualityComparer<int?>.Default.Equals(PlayerId, other.PlayerId);
        }

        public virtual void ReadXml(XmlReader reader)
        {
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
            writer.WriteAttributeString("timestamp", Timestamp.ToString("s"));

            if (PlayerId.HasValue)
                writer.WriteAttributeString("playerId", PlayerId.ToString());
        }

        public virtual XmlSchema GetSchema()
        {
            return null;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Piece);
        }

        public override int GetHashCode()
        {
            var hashCode = -2135171675;
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + Type.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<int?>.Default.GetHashCode(PlayerId);
            hashCode = hashCode * -1521134295 + Timestamp.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(Piece piece1, Piece piece2)
        {
            return EqualityComparer<Piece>.Default.Equals(piece1, piece2);
        }

        public static bool operator !=(Piece piece1, Piece piece2)
        {
            return !(piece1 == piece2);
        }

        public override string ToString()
        {
            return $"Id:{Id}, {Type}, PlayerId:{PlayerId}";
        }
    }
}