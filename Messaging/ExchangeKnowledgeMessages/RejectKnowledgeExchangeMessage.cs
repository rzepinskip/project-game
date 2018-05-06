using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Interfaces;

namespace Messaging.ExchangeKnowledgeMessages
{
    public class RejectKnowledgeExchangeMessage : KnowledgeExchangeMessage
    {
        public bool Permanent { get; set; }
        public override IMessage Process(IGameMaster gameMaster)
        {
            return this;
        }

        public override void Process(IPlayer player)
        {
            player.HandleExchangeKnowledge(SenderPlayerId);
        }

        public override void Process(ICommunicationServer cs, int id)
        {
            var gameId = cs.GetGameIdForPlayer(id);
            cs.Send(this, gameId == id ? PlayerId : gameId);
        }
    }
}