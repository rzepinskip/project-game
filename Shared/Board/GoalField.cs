using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Shared.Board
{
    [Serializable]
    public class GoalField : Field, IEquatable<GoalField>
    {
        [XmlAttribute("type")]
        public Shared.CommonResources.GoalFieldType Type { get; set; }

        [XmlAttribute("team")]
        public Shared.CommonResources.Team Team { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as GoalField);
        }

        public bool Equals(GoalField other)
        {
            return other != null &&
                   base.Equals(other) &&
                   Type == other.Type &&
                   Team == other.Team;
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
