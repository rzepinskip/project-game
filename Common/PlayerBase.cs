using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Common
{
    public class PlayerBase : IXmlSerializable, IEquatable<PlayerBase>
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

        public override bool Equals(object obj)
        {
            return Equals(obj as PlayerBase);
        }

        public bool Equals(PlayerBase other)
        {
            return other != null &&
                   Id == other.Id &&
                   Team == other.Team &&
                   Role == other.Role;
        }

        public override int GetHashCode()
        {
            var hashCode = 525671237;
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + Team.GetHashCode();
            hashCode = hashCode * -1521134295 + Role.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(PlayerBase base1, PlayerBase base2)
        {
            return EqualityComparer<PlayerBase>.Default.Equals(base1, base2);
        }

        public static bool operator !=(PlayerBase base1, PlayerBase base2)
        {
            return !(base1 == base2);
        }
    }
}