namespace Common
{
    public class GameInfo
    {
        public string GameName { get; set; }
        public int BlueTeamPlayers { get; set; }
        public int RedTeamPlayers { get; set; }

        public GameInfo(string gameName, int blueTeamPlayers, int redTeamPlayers)
        {
            GameName = gameName;
            BlueTeamPlayers = blueTeamPlayers;
            RedTeamPlayers = redTeamPlayers;
        }
    }
}
