using System;
using System.Xml.Serialization;
using Common.Interfaces;

namespace Messaging.KnowledgeExchangeMessages
{
    [XmlType(XmlRootName)]
    public class KnowledgeExchangeRequestMessage : BetweenPlayersMessage
    {
        public const string XmlRootName = "KnowledgeExchangeRequest";

        public KnowledgeExchangeRequestMessage(int playerId, int senderPlayerId) : base(playerId, senderPlayerId)
        {
        }

        protected KnowledgeExchangeRequestMessage()
        {
        }

        public override IMessage Process(IGameMaster gameMaster)
        {
            throw new NotImplementedException();
        }

        public override void Process(IPlayer player)
        {
            // Strategy handles this message
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