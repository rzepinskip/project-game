using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Shared.BoardObjects
{
    [Serializable]
    [DebuggerDisplay("[X = {X}, Y = {Y}]")]
    public class Location : IEquatable<Location>
    {
        public Location(int x, int y)
        {
            X = x;
            Y = y;
        }
        [XmlAttribute("x")]
        public int X { get; set; }

        [XmlAttribute("y")]
        public int Y { get; set; }

        public Location()
        {
            
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as Location);
        }

        public bool Equals(Location other)
        {
            return other != null &&
                   X == other.X &&
                   Y == other.Y;
        }

        public Location GetNewLocation( CommonResources.MoveType direction)
        {
            var nl = new Location(){ X = X, Y = Y };
            switch (direction)
            {
                case CommonResources.MoveType.Down:
                    nl.Y = Y - 1;
                    break;
                case CommonResources.MoveType.Left:
                    nl.X = X - 1;
                    break;
                case CommonResources.MoveType.Right:
                    nl.X = X + 1;
                    break;
                case CommonResources.MoveType.Up:
                    nl.Y = Y + 1;
                    break;
            }
            return nl;
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
    }
}
