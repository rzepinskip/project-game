using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace Common
{
    public static class StringExtension
    {
        public static T GetEnumValueFor<T>(this string str) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum) throw new ArgumentException("T must be an enum");

            var members = typeof(T).GetMembers();
            var map = new Dictionary<string, T>();
            foreach (var member in members)
            {
                if (!(member.GetCustomAttributes(typeof(XmlEnumAttribute), false).FirstOrDefault() is XmlEnumAttribute
                    enumAttrib))
                    continue;

                var xmlEnumValue = enumAttrib.Name;
                var enumVal = ((FieldInfo) member).GetRawConstantValue();
                map.Add(xmlEnumValue, (T) enumVal);
            }

            return map[str];
        }
    }

    public static class EnumExtensions
    {
        public static string GetXmlAttributeName<T>(this T enumVal)
        {
            var type = enumVal.GetType();
            var info = type.GetField(Enum.GetName(typeof(T), enumVal));
            var att = (XmlEnumAttribute) info.GetCustomAttributes(typeof(XmlEnumAttribute), false)[0];

            return att.Name;
        }
    }

    public enum ActionType
    {
        Move,
        Discover,
        PickUp,
        Place,
        Test,
        Destroy
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

        [XmlEnum("normal")] Normal,

        [XmlEnum("destroyed")] Destroyed
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