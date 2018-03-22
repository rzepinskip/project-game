using Shared.BoardObjects;

namespace Shared
{
    public class PlayerInfo
    {
        public PlayerInfo()
        {
        }

        public PlayerInfo(CommonResources.TeamColour team, PlayerBase.PlayerType role, Location location,
            Piece piece = null)
        {
            Team = team;
            Role = role;
            Location = location;
            Piece = piece;
        }

        public CommonResources.TeamColour Team { get; set; }
        public PlayerBase.PlayerType Role { get; set; }
        public Location Location { get; set; }
        public Piece Piece { get; set; }
    }
}