using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Common.Interfaces;

namespace Messaging.ExchangeKnowledgeMessages
{
    [XmlType(XmlRootName)]
    public class RejectKnowledgeExchangeMessage : BetweenPlayersMessage
    {
        public const string XmlRootName = "RejectKnowledgeExchange";
        public RejectKnowledgeExchangeMessage(int senderId, int withPlayerId, bool permanent = false): base(senderId, withPlayerId)
        {
            Permanent = permanent;
        }

        public bool Permanent { get; set; }
        public override IMessage Process(IGameMaster gameMaster)
        {
            gameMaster.KnowledgeExchangeManager.HandleExchangeRejection(subjectId: SenderPlayerId, initiatorId: PlayerId);
            return this;
        }

        public override void Process(IPlayer player)
        {
            player.HandleKnowledgeExchangeRequest(SenderPlayerId);
        }

        public override void Process(ICommunicationServer cs, int id)
        {
            var gameId = cs.GetGameIdFor(id);
            cs.Send(this, gameId == id ? PlayerId : gameId);
        }
        public override string ToLog()
        {
            return XmlRootName + base.ToLog();
        }
    }
}