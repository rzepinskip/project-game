using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Common.ActionInfo;
using Common.Interfaces;
using Messaging.Responses;

namespace Messaging.Requests
{
    public abstract class Request : Message, IRequest, IEquatable<Request>
    {
        protected Request()
        {
        }

        protected Request(Guid playerGuid, int gameId)
        {
            PlayerGuid = playerGuid;
            GameId = gameId;
        }

        [XmlAttribute("gameId")] public int GameId { get; set; }

        public bool Equals(Request other)
        {
            return other != null &&
                   GameId == other.GameId &&
                   PlayerGuid == other.PlayerGuid;
        }

        [XmlAttribute("playerGuid")] public Guid PlayerGuid { get; set; }

        public override IMessage Process(IGameMaster gameMaster)
        {
            var result = gameMaster.EvaluateAction(GetActionInfo());

            return ResponseWithData.ConvertToData(result.data, result.isGameFinished);
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
            return string.Join(',', PlayerGuid, GameId);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Request);
        }

        public override int GetHashCode()
        {
            var hashCode = -497176057;
            hashCode = hashCode * -1521134295 + GameId.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Guid>.Default.GetHashCode(PlayerGuid);
            return hashCode;
        }

        public static bool operator ==(Request request1, Request request2)
        {
            return EqualityComparer<Request>.Default.Equals(request1, request2);
        }

        public static bool operator !=(Request request1, Request request2)
        {
            return !(request1 == request2);
        }
    }
}