using Common.BoardObjects;

namespace Messaging.Responses
{
    internal class TestPieceResponse : Response
    {
        public int PlayerId { get; set; }
        public Piece Piece { get; set; }
    }
}