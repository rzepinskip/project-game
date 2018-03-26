using Common.BoardObjects;

namespace Common
{
    public class PlayerInfo
    {
        public PlayerInfo(TeamColor team, PlayerType role, Location location,
            Piece piece = null)
        {
            Team = team;
            Role = role;
            Location = location;
            Piece = piece;
        }

        public TeamColor Team { get; set; }
        public PlayerType Role { get; set; }
        public Location Location { get; set; }
        public Piece Piece { get; set; }
    }
}