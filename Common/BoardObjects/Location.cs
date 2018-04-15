using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Common.BoardObjects
{
    [DebuggerDisplay("[X = {X}, Y = {Y}]")]
    public class Location : IXmlSerializable, IEquatable<Location>
    {
        protected Location()
        {
        }

        public Location(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }

        public int Y { get; set; }

        public bool Equals(Location other)
        {
            return other != null &&
                   X == other.X &&
                   Y == other.Y;
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public virtual void ReadXml(XmlReader reader)
        {
            reader.MoveToContent();
            X = int.Parse(reader.GetAttribute("x"));
            Y = int.Parse(reader.GetAttribute("y"));

            var isEmptyElement = reader.IsEmptyElement;
            reader.ReadStartElement(); 
            if (!isEmptyElement) reader.ReadEndElement();
        }

        public virtual void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("x", X.ToString());
            writer.WriteAttributeString("y", Y.ToString());
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Location);
        }

        public Location GetNewLocation(Direction direction)
        {
            Location nl = null;

            switch (direction)
            {
                case Direction.Down:
                    nl = new Location(X, Y - 1);
                    break;
                case Direction.Left:
                    nl = new Location(X - 1, Y);
                    break;
                case Direction.Right:
                    nl = new Location(X + 1, Y);
                    break;
                case Direction.Up:
                    nl = new Location(X, Y + 1);
                    break;
                default:
                    break;
            }

            return nl;
        }

        public Direction GetDirectionFrom(Location location)
        {
            var dx = X - location.X;
            var dy = Y - location.Y;
            var horizontalDirection = dx > 0 ? Direction.Right : Direction.Left;
            var verticalDirection = dy > 0 ? Direction.Up : Direction.Down;

            if (dx == 0 && dy == 0)
                throw new Exception("You're already on that field dummy !");

            if (dx == 0)
                return verticalDirection;

            if (dy == 0)
                return horizontalDirection;

            var random = new Random();
            return random.Next() % 2 == 0 ? verticalDirection : horizontalDirection;
        }

        public override int GetHashCode()
        {
            var hashCode = 1861411795;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(Location location1, Location location2)
        {
            return EqualityComparer<Location>.Default.Equals(location1, location2);
        }

        public static bool operator !=(Location location1, Location location2)
        {
            return !(location1 == location2);
        }

        public Direction DirectionToTask(TeamColor team)
        {
            return team == TeamColor.Red
                ? Direction.Down
                : Direction.Up;
        }

        public Direction DirectionToGoal(TeamColor team)
        {
            return team == TeamColor.Red
                ? Direction.Up
                : Direction.Down;
        }

        public override string ToString()
        {
            return "(" + X + ", " + Y + ")";
        }
    }
}