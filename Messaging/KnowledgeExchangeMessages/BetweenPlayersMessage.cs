using System.Xml.Serialization;

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

        [XmlAttribute("senderPlayerId")] public int SenderPlayerId { get; set; }
    }
}