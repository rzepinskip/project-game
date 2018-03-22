using Common.Interfaces;

namespace Messaging.Responses
{
    public abstract class Response
    {
        public Response(int playerId, bool isGameFinished = false)
        {
            PlayerId = playerId;
            IsGameFinished = isGameFinished;
        }

        public int PlayerId { get; }
        public bool IsGameFinished { get; set; }

        public abstract void Update(IBoard board);
    }
}