using System.Xml.Serialization;

namespace Common
{
    public class GameInfo
    {
        protected GameInfo()
        {
        }

        public GameInfo(string gameName, int blueTeamPlayers, int redTeamPlayers)
        {
            GameName = gameName;
            BlueTeamPlayers = blueTeamPlayers;
            RedTeamPlayers = redTeamPlayers;
        }

        [XmlAttribute("gameName")] public string GameName { get; set; }
        [XmlAttribute("blueTeamPlayers")] public int BlueTeamPlayers { get; set; }
        [XmlAttribute("redTeamPlayers")] public int RedTeamPlayers { get; set; }
    }
}