using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Common.Interfaces;

namespace Messaging.ExchangeKnowledgeMessages
{
    [XmlType(XmlRootName)]
    public class AcceptExchangeRequestMessage : KnowledgeExchangeMessage
    {
        public const string XmlRootName = "AcceptExchangeRequest";
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
            var gameId = cs.GetGameIdFor(id);
            cs.Send(this, gameId == id ? PlayerId : gameId);
        }
    }
}
