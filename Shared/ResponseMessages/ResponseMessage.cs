using Shared.BoardObjects;

namespace Shared.ResponseMessages
{
    public abstract class ResponseMessage
    {
        public int PlayerId { get; set; }
        public bool GameFinished { get; set; }

        public abstract void Update(Board board);
    }
}