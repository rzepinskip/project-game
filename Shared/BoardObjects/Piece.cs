using System;

namespace Shared.BoardObjects
{
    public class Piece
    {
        public int Id { get; set; }
        public CommonResources.PieceType Type { get; set; }
        public DateTime TimeStamp { get; set; }
        public int? PlayerId { get; set; }

        public Piece()
        { }

        public Piece(int id, CommonResources.PieceType type, DateTime timeStamp =  default(DateTime), int? playerId = null)
        {
            Id = id;
            Type = type;
            TimeStamp = timeStamp != null ? timeStamp : DateTime.Now;
            PlayerId = playerId;
        }
    }
}
