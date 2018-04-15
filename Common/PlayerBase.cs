using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Common
{
    public class PlayerBase : IXmlSerializable
    {
        protected PlayerBase()
        {
        }

        public PlayerBase(int id, TeamColor team, PlayerType role = PlayerType.Member)
        {
            Id = id;
            Team = team;
            Role = role;
        }

        public int Id { get; set; }
        public TeamColor Team { get; set; }
        public PlayerType Role { get; set; }

        public virtual XmlSchema GetSchema()
        {
            return null;
        }

        public virtual void ReadXml(XmlReader reader)
        {
            Id = int.Parse(reader.GetAttribute("id"));
            Team = reader.GetAttribute("team").GetEnumValueFor<TeamColor>();
            Role = reader.GetAttribute("role").GetEnumValueFor<PlayerType>();
        }

        public virtual void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("id", Id.ToString());
            writer.WriteAttributeString("team", Team.GetXmlAttributeName());
            writer.WriteAttributeString("role", Role.GetXmlAttributeName());
        }
    }
}