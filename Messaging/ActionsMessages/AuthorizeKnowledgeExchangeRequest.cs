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
    public class AuthorizeKnowledgeExchangeRequest : RequestMessage
    {
        public const string XmlRootName = "AuthorizeKnowledgeExchange";

        public AuthorizeKnowledgeExchangeRequest()
        {
        }

        public AuthorizeKnowledgeExchangeRequest(Guid playerGuid, int gameId, int withPlayerId) : base(playerGuid, gameId)
        {
            WithPlayerId = withPlayerId;
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
            if (PlayerGuid == default(Guid))
            {
                //Console.WriteLine("Unsigned request");
                return null;
            }

            var optionalSenderId = gameMaster.Authorize(PlayerGuid);

            if (!optionalSenderId.HasValue)
            {
                //Console.WriteLine("Unrecognized player");
                return null;
            }

            var senderId = optionalSenderId.Value;

            //Console.WriteLine($"Player {senderId} request to {WithPlayerId}");

            if (!gameMaster.PlayerIdExists(WithPlayerId))
                return new RejectKnowledgeExchangeMessage(senderId, WithPlayerId, null, true);
            gameMaster.EvaluateAction(GetActionInfo());
            return null;
        }

        public override void Process(ICommunicationServer cs, int id)
        {
            cs.Send(this, GameId);
        }
    }
}