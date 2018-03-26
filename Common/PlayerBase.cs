namespace Common
{
    public class PlayerBase
    {
        public PlayerBase()
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
    }
}