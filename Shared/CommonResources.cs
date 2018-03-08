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
            [XmlEnum("non-goal")]
            NonGoal,
            [XmlEnum("goal")]
            Goal,
            [XmlEnum("unknown")]
            Unknown
        }

        public enum TeamColour
        {
            [XmlEnum("red")]
            Red,
            [XmlEnum("blue")]
            Blue
        }

        public enum MoveType
        {
            [XmlEnum("up")]
            Up,
            [XmlEnum("down")]
            Down,
            [XmlEnum("left")]
            Left,
            [XmlEnum("right")]
            Right
        }

        public enum PieceType
        {
            [XmlEnum("unknown")]
            Unknown,
            [XmlEnum("sham")]
            Sham,
            [XmlEnum("normal")]
            Normal
        }
    }
}
