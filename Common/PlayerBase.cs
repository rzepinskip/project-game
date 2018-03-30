using System;
using System.Xml.Serialization;

namespace Common
{
    [Serializable]
    public class PlayerBase
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
        [XmlAttribute("team")] public TeamColor Team { get; set; }
        public PlayerType Role { get; set; }
    }
}