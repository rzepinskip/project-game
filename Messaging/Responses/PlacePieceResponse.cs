using Common.BoardObjects;

namespace Messaging.Responses
{
    internal class PlacePieceResponse : Response
    {
        public int PlayerId { get; internal set; }
        public GoalField GoalField { get; internal set; }
    }
}