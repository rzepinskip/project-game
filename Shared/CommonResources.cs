using System.Xml.Serialization;

namespace Shared
{
    public static class CommonResources
    {
        public enum ActionType
        {
            Move,
            Discover,
            PickUp,
            Place,
            Test
        }

        public enum GoalFieldType
        {
            [XmlEnum("non-goal")] NonGoal,
            [XmlEnum("goal")] Goal,
            [XmlEnum("unknown")] Unknown
        }

        public enum MoveType
        {
            [XmlEnum("up")] Up,
            [XmlEnum("down")] Down,
            [XmlEnum("left")] Left,
            [XmlEnum("right")] Right
        }

        public enum PieceType
        {
            [XmlEnum("unknown")] Unknown,
            [XmlEnum("sham")] Sham,
            [XmlEnum("normal")] Normal
        }

        public enum TeamColour
        {
            [XmlEnum("red")] Red,
            [XmlEnum("blue")] Blue
        }
    }
}