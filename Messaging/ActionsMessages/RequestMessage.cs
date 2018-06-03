using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Common.ActionInfo;
using Common.Interfaces;

namespace Messaging.Requests
{
    public abstract class RequestMessage : Message, IRequestMessage, IEquatable<RequestMessage>
    {
        protected RequestMessage()
        {
        }

        protected RequestMessage(Guid playerGuid, int gameId)
        {
            PlayerGuid = playerGuid;
            GameId = gameId;
        }

        [XmlAttribute("gameId")] public int GameId { get; set; }

        public bool Equals(RequestMessage other)
        {
            return other != null &&
                   GameId == other.GameId &&
                   PlayerGuid == other.PlayerGuid;
        }

        [XmlAttribute("playerGuid")] public Guid PlayerGuid { get; set; }

        public override IMessage Process(IGameMaster gameMaster)
        {
            var result = gameMaster.EvaluateAction(GetActionInfo());

            return DataMessage.FromBoardDataOverridingTimestamps(result.data, result.isGameFinished);
        }

        public override void Process(IPlayer player)
        {
            throw new InvalidOperationException();
        }

        public override void Process(ICommunicationServer cs, int id)
        {
            cs.Send(this, GameId);
        }

        public abstract ActionInfo GetActionInfo();

        public override string ToLog()
        {
            return string.Join(',', DateTime.Now, GameId);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as RequestMessage);
        }

        public override int GetHashCode()
        {
            var hashCode = -497176057;
            hashCode = hashCode * -1521134295 + GameId.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Guid>.Default.GetHashCode(PlayerGuid);
            return hashCode;
        }

        public static bool operator ==(RequestMessage request1, RequestMessage request2)
        {
            return EqualityComparer<RequestMessage>.Default.Equals(request1, request2);
        }

        public static bool operator !=(RequestMessage request1, RequestMessage request2)
        {
            return !(request1 == request2);
        }
    }
}