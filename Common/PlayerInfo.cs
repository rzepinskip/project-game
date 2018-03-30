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

        public PlayerInfo(PlayerBase basePlayer, Location location, Piece piece = null) : base(basePlayer.Id,
            basePlayer.Team, basePlayer.Role)
        {
            Location = location;
            Piece = piece;
        }

        public Location Location { get; set; }
        public Piece Piece { get; set; }
    }
}