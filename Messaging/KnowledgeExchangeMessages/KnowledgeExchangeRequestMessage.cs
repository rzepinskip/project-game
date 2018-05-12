using System;
using System.Collections.Generic;
using System.Text;
using Common.Interfaces;
using System.Xml.Serialization;

namespace Messaging.ExchangeKnowledgeMessages
{
    [XmlType(XmlRootName)]
    public class KnowledgeExchangeRequestMessage : BetweenPlayersMessage
    {
        public const string XmlRootName = "KnowledgeExchangeRequest";

        public KnowledgeExchangeRequestMessage(int playerId, int senderPlayerId) : base(playerId, senderPlayerId)
        {
        }

        public override IMessage Process(IGameMaster gameMaster)
        {
            throw new NotImplementedException();
        }

        public override void Process(IPlayer player)
        {
            player.HandleKnowledgeExchangeRequest(SenderPlayerId);
        }

        public override void Process(ICommunicationServer cs, int id)
        {
            cs.Send(this, PlayerId);
        }

        public override string ToLog()
        {
            return XmlRootName + base.ToLog();
        }
    }
}