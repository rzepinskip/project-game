using System;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Common.BoardObjects
{
    [DebuggerDisplay("[Id = {Id}, PlayerId = {PlayerId}]")]
    [Serializable]
    public class Piece
    {
        protected Piece()
        { }

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
        [XmlAttribute("type")] public PieceType Type { get; set; }
        public int? PlayerId { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}