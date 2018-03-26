using System;

namespace Common.BoardObjects
{
    public class Piece
    {
        public Piece(int id, PieceType type, int? playerId = null) : this(id, type, playerId, DateTime.Now)
        {
        }

        public Piece(int id, PieceType type, int? playerId, DateTime timeStamp)
        {
            Id = id;
            Type = type;
            TimeStamp = timeStamp;
            PlayerId = playerId;
        }

        public int Id { get; }
        public PieceType Type { get; set; }
        public int? PlayerId { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}