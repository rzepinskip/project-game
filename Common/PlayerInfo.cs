using Common.BoardObjects;

namespace Common
{
    public class PlayerInfo : PlayerBase
    {
        public PlayerInfo(int playerId, TeamColor team, PlayerType role, Location location,
            Piece piece = null) : base(playerId, team, role)
        {
            Location = location;
            Piece = piece;
        }

        public Location Location { get; set; }
        public Piece Piece { get; set; }
    }
}