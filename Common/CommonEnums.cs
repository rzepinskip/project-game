using System.Xml.Serialization;

namespace Common
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

    public enum Direction
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

    public enum TeamColor
    {
        [XmlEnum("red")] Red,

        [XmlEnum("blue")] Blue
    }

    public enum PlayerType
    {
        [XmlEnum(Name = "member")] Member,

        [XmlEnum(Name = "leader")] Leader
    }
}