using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Common.BoardObjects
{
    [Serializable]
    public class GoalField : Field, IEquatable<GoalField>
    {
        public GoalField(Location location, TeamColor team, GoalFieldType type = GoalFieldType.Unknown,
            int? player = null) : this(location, team, type, player, DateTime.Now)
        {
        }

        public GoalField(Location location, TeamColor team, GoalFieldType type, int? playerId, DateTime timestamp) :
            base(location, playerId, timestamp)
        {
            Team = team;
            Type = type;
        }

        [XmlAttribute("type")]
        public GoalFieldType Type { get; set; }

        [XmlAttribute("team")]
        public TeamColor Team { get; }

        public bool Equals(GoalField other)
        {
            return other != null &&
                   base.Equals(other) &&
                   Type == other.Type &&
                   Team == other.Team;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as GoalField);
        }

        public override int GetHashCode()
        {
            var hashCode = -1655874655;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + Type.GetHashCode();
            hashCode = hashCode * -1521134295 + Team.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(GoalField field1, GoalField field2)
        {
            return EqualityComparer<GoalField>.Default.Equals(field1, field2);
        }

        public static bool operator !=(GoalField field1, GoalField field2)
        {
            return !(field1 == field2);
        }
    }
}