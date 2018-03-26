using Common.Interfaces;
using Messaging.ActionHelpers;

namespace Messaging.Responses
{
    public abstract class Response : IResponse, ILoggable
    {
        public Response(int playerId, bool isGameFinished = false)
        {
            PlayerId = playerId;
            IsGameFinished = isGameFinished;
        }

        public int PlayerId { get; set; }
        public bool IsGameFinished { get; set; }

        public virtual string ToLog()
        {
            return string.Join(',', PlayerId, IsGameFinished);
        }

        public virtual IMessage Process(IGameMaster gameMaster)
        {
            throw new System.NotImplementedException();
        }

        public virtual void Process(IPlayer player)
        {
            throw new System.NotImplementedException();
        }
    }
}