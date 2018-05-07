using System;
using System.Collections.Generic;
using System.Xml;

namespace Common.BoardObjects
{
    public abstract class Field : Location, IEquatable<Field>
    {
        protected Field()
        {
        }

        public Field(Location location) : this(location, null, DateTime.Now)
        {
        }

        protected Field(Location location, int? playerId, DateTime timestamp) : base(location.X, location.Y)
        {
            PlayerId = playerId;
            Timestamp = timestamp;
        }

        public int? PlayerId { get; set; }
        public DateTime Timestamp { get; set; }

        public bool Equals(Field other)
        {
            return other != null &&
                   base.Equals(other) &&
                   EqualityComparer<int?>.Default.Equals(PlayerId, other.PlayerId);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Field);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(Field field1, Field field2)
        {
            return EqualityComparer<Field>.Default.Equals(field1, field2);
        }

        public static bool operator !=(Field field1, Field field2)
        {
            return !(field1 == field2);
        }

        public override void ReadXml(XmlReader reader)
        {
            if (!string.IsNullOrWhiteSpace(reader.GetAttribute("timestamp")))
                Timestamp = DateTime.Parse(reader.GetAttribute("timestamp"));

            if (!string.IsNullOrWhiteSpace(reader.GetAttribute("playerId")))
                PlayerId = int.Parse(reader.GetAttribute("playerId"));

            base.ReadXml(reader);
        }

        public override void WriteXml(XmlWriter writer)
        {
            base.WriteXml(writer);

            writer.WriteAttributeString("timestamp", Timestamp.ToString("s"));

            if (PlayerId.HasValue)
                writer.WriteAttributeString("playerId", PlayerId.ToString());
        }

        public override string ToString()
        {
            return base.ToString() + $", PlayerId={PlayerId}";
        }
    }
}