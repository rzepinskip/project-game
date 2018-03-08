using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Shared
{
    public static class CommonResources
    {
        public enum GoalFieldType
        {
            [XmlEnum(Name = "non-goal")]
            NonGoal,
            [XmlEnum(Name = "goal")]
            Goal,
            [XmlEnum(Name = "unknown")]
            Unknown
        }
        public enum Team {
            [XmlEnum(Name = "red")]
            Red,
            [XmlEnum(Name = "blue")]
            Blue
        }
        public enum MoveType {
            [XmlEnum(Name = "up")]
            Up,
            [XmlEnum(Name = "down")]
            Down,
            [XmlEnum(Name = "left")]
            Left,
            [XmlEnum(Name = "right")]
            Right
        }
        public enum PieceType {
            [XmlEnum(Name = "unknown")]
            Unknown,
            [XmlEnum(Name = "sham")]
            Sham,
            [XmlEnum(Name = "normal")]
            Normal
        }
    }
}
