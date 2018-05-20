using System;
using System.Xml.Serialization;
using Common.Interfaces;

namespace Messaging.KnowledgeExchangeMessages
{
    [XmlType(XmlRootName)]
    public class RejectKnowledgeExchangeMessage : BetweenPlayersMessage
    {
        public const string XmlRootName = "RejectKnowledgeExchange";

        protected RejectKnowledgeExchangeMessage()
        {
        }

        public RejectKnowledgeExchangeMessage(int senderId, int withPlayerId, Guid? playerGuid, bool permanent = false) : base(senderId,
            withPlayerId)
        {
            Permanent = permanent;
            PlayerGuid = playerGuid;
        }

        [XmlAttribute("permanent")] public bool Permanent { get; set; }
        [XmlIgnore] public Guid? PlayerGuid { get; set; }

        [XmlAttribute("playerGuid")]
        public Guid PlayerGuidValue
        {
            get
            {
                if (PlayerGuid != null) return PlayerGuid.Value;

                throw new InvalidOperationException();
            }
            set => PlayerGuid = value;
        }
        [XmlIgnore] public bool PlayerGuidValueSpecified => PlayerGuid.HasValue;

        public override IMessage Process(IGameMaster gameMaster)
        {
            var promisedRejecterId = PlayerGuid.HasValue ? gameMaster.Authorize(PlayerGuid.Value) : null;
            if (!promisedRejecterId.HasValue) return null;
            SenderPlayerId = promisedRejecterId.Value;
            PlayerGuid = null;
            gameMaster.KnowledgeExchangeManager.HandleExchangeRejection(SenderPlayerId, PlayerId);
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