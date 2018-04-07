using System;
using System.Collections.Generic;
using System.Xml;

namespace Common.BoardObjects
{
    public class GoalField : Field, IEquatable<GoalField>
    {
        protected GoalField()
        {
        }

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

        public GoalFieldType Type { get; set; }

        public TeamColor Team { get; set; }

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

        public override void ReadXml(XmlReader reader)
        {
            base.ReadXml(reader);

            if (Enum.TryParse(reader.GetAttribute("type"), out GoalFieldType type))
                Type = type;

            if (Enum.TryParse(reader.GetAttribute("team"), out TeamColor team))
                Team = team;
        }

        public override void WriteXml(XmlWriter writer)
        {
            base.WriteXml(writer);

            writer.WriteAttributeString("type", Type.ToString());
            writer.WriteAttributeString("team", Team.ToString());
        }
    }
}