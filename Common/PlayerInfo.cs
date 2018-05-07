using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using Common.BoardObjects;

namespace Common
{
    public class PlayerInfo : PlayerBase, IEquatable<PlayerInfo>
    {
        protected PlayerInfo()
        {
        }

        public PlayerInfo(int playerId, TeamColor team, PlayerType role, Location location = null,
            Piece piece = null) : base(playerId, team, role)
        {
            Location = location;
            Piece = piece;
        }

        public PlayerInfo(PlayerBase basePlayer, Location location = null, Piece piece = null) : base(basePlayer.Id,
            basePlayer.Team, basePlayer.Role)
        {
            Location = location;
            Piece = piece;
        }

        public Location Location { get; set; }
        public Piece Piece { get; set; }

        public bool Equals(PlayerInfo other)
        {
            return other != null &&
                   base.Equals(other) &&
                   EqualityComparer<Location>.Default.Equals(Location, other.Location) &&
                   EqualityComparer<Piece>.Default.Equals(Piece, other.Piece);
        }

        public override void ReadXml(XmlReader reader)
        {
            ReadContent(reader);

            reader.ReadStartElement();

            var locationSerializer = new XmlSerializer(typeof(Location));
            Location = locationSerializer.Deserialize(reader) as Location;

            if (reader.NodeType != XmlNodeType.EndElement)
            {
                var pieceSerializer = new XmlSerializer(typeof(Piece));
                Piece = pieceSerializer.Deserialize(reader) as Piece;
            }

            reader.ReadEndElement();
        }

        public override void WriteXml(XmlWriter writer)
        {
            base.WriteXml(writer);

            var locationSerializer = new XmlSerializer(typeof(Location));
            locationSerializer.Serialize(writer, Location);

            if (Piece != null)
            {
                var pieceSerializer = new XmlSerializer(typeof(Piece));
                pieceSerializer.Serialize(writer, Piece);
            }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as PlayerInfo);
        }

        public override int GetHashCode()
        {
            var hashCode = -14891557;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Location>.Default.GetHashCode(Location);
            hashCode = hashCode * -1521134295 + EqualityComparer<Piece>.Default.GetHashCode(Piece);
            return hashCode;
        }

        public static bool operator ==(PlayerInfo info1, PlayerInfo info2)
        {
            return EqualityComparer<PlayerInfo>.Default.Equals(info1, info2);
        }

        public static bool operator !=(PlayerInfo info1, PlayerInfo info2)
        {
            return !(info1 == info2);
        }

        public override string ToString()
        {
            return $"Id:{Id}, {Team}, {Role}, {Location}, Piece:{{{Piece}}}]";
        }
    }
}