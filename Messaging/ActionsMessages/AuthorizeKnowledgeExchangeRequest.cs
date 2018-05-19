using System;
using System.Xml.Serialization;
using Common;
using Common.ActionInfo;
using Common.Interfaces;
using Messaging.KnowledgeExchangeMessages;
using Messaging.Requests;

namespace Messaging.ActionsMessages
{
    [XmlType(XmlRootName)]
    public class AuthorizeKnowledgeExchangeRequest : Request
    {
        public const string XmlRootName = "AuthorizeKnowledgeExchange";

        public AuthorizeKnowledgeExchangeRequest()
        {
        }

        public AuthorizeKnowledgeExchangeRequest(Guid playerGuid, int gameId) : base(playerGuid, gameId)
        {
        }

        [XmlAttribute("withPlayerId")] public int WithPlayerId { get; set; }

        public override ActionInfo GetActionInfo()
        {
            return new KnowledgeExchangeInfo(PlayerGuid, WithPlayerId);
        }

        public override string ToLog()
        {
            return string.Join(',', ActionType.AuthorizeKnowledgeExchange, base.ToLog());
        }

        public override IMessage Process(IGameMaster gameMaster)
        {
            var optionalSenderId = gameMaster.Authorize(PlayerGuid);
            if (!optionalSenderId.HasValue) throw new ApplicationFatalException();
            var senderId = optionalSenderId.Value;
            if (!gameMaster.PlayerIdExists(WithPlayerId))
                return new RejectKnowledgeExchangeMessage(senderId, WithPlayerId, true);
            gameMaster.EvaluateAction(GetActionInfo());
            return null;
        }
    }
}