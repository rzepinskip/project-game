using System;
using System.Collections.Generic;
using System.Text;
using Common.Interfaces;

namespace Messaging.ExchangeKnowledgeMessages
{
    public abstract class BetweenPlayersMessage : Message
    {
        protected BetweenPlayersMessage()
        {
        }

        protected BetweenPlayersMessage(int playerId, int senderPlayerId)
        {
            PlayerId = playerId;
            SenderPlayerId = senderPlayerId;
        }

        public int PlayerId { get; set; }
        public int SenderPlayerId { get; set; }
    }
}