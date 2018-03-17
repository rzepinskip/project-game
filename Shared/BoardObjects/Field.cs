using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Shared.BoardObjects
{
    [Serializable]
    public abstract class Field : Location, IEquatable<Field>
    { 
        public Field():base(0,0) { }
        public Field(int? playerId, DateTime timestamp, int x, int y) : base(x,y)
        {
            PlayerId = playerId;
            Timestamp = timestamp;
        }

        //public Location Location { get; set; }
        public int? PlayerId { get; set; }
        public DateTime Timestamp { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as Field);
        }

        public bool Equals(Field other)
        {
            return other != null &&
                   base.Equals(other) &&
                   EqualityComparer<int?>.Default.Equals(PlayerId, other.PlayerId) &&
                   Timestamp == other.Timestamp;
        }

        public override int GetHashCode()
        {
            var hashCode = 1655836488;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<int?>.Default.GetHashCode(PlayerId);
            hashCode = hashCode * -1521134295 + Timestamp.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(Field field1, Field field2)
        {
            return EqualityComparer<Field>.Default.Equals(field1, field2);
        }

        public static bool operator !=(Field field1, Field field2)
        {
            return !(field1 == field2);
        }
    }
}
