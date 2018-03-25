using Common.Interfaces;
using Messaging.ActionHelpers;

namespace Messaging.Responses
{
    public abstract class Response : ILoggable
    {
        public Response(int playerId, bool isGameFinished = false)
        {
            PlayerId = playerId;
            IsGameFinished = isGameFinished;
        }

        public int PlayerId { get; }
        public bool IsGameFinished { get; set; }

        public abstract void Update(IBoard board);
        public virtual string ToLog()
        {
            return string.Join(',', PlayerId, IsGameFinished);
        }
    }
}