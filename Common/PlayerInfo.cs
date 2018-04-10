using System.Xml;
using System.Xml.Serialization;
using Common.BoardObjects;

namespace Common
{
    public class PlayerInfo : PlayerBase
    {
        protected PlayerInfo()
        {

        }

        public PlayerInfo(int playerId, TeamColor team, PlayerType role, Location location,
            Piece piece = null) : base(playerId, team, role)
        {
            Location = location;
            Piece = piece;
        }

        public PlayerInfo(PlayerBase basePlayer, Location location, Piece piece = null) : base(basePlayer.Id,
            basePlayer.Team, basePlayer.Role)
        {
            Location = location;
            Piece = piece;
        }

        public Location Location { get; set; }
        public Piece Piece { get; set; }

        public override void ReadXml(XmlReader reader)
        {
            base.ReadXml(reader);

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
    }
}