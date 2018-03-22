using Common.BoardObjects;

namespace Messaging.Responses
{
    internal class PickUpPieceResponse : Response
    {
        public PickUpPieceResponse()
        {
        }

        public int PlayerId { get; internal set; }
        public Piece Piece { get; internal set; }
    }
}