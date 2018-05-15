namespace Messaging.KnowledgeExchangeMessages
{
    public abstract class BetweenPlayersMessage : MessageToPlayer
    {
        protected BetweenPlayersMessage()
        {
        }

        protected BetweenPlayersMessage(int playerId, int senderPlayerId) : base(playerId)
        {
            SenderPlayerId = senderPlayerId;
        }

        public int SenderPlayerId { get; set; }
    }
}