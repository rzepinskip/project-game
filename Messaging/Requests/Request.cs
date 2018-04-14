using System;
using System.Xml.Serialization;
using Common.ActionInfo;
using Common.Interfaces;
using Messaging.Responses;

namespace Messaging.Requests
{
    public abstract class Request : Message, IRequest
    {
        protected Request()
        {
        }

        protected Request(string playerGuid)
        {
            PlayerGuid = playerGuid;
        }

        [XmlAttribute("gameId")] public int GameId { get; set; }

        [XmlAttribute("playerGuid")] public string PlayerGuid { get; set; }

        public override IMessage Process(IGameMaster gameMaster)
        {
            var result = gameMaster.EvaluateAction(GetActionInfo());
            return ResponseWithData.ConvertToData(result.data, result.isGameFinished);
        }

        public override void Process(IPlayer player)
        {
            throw new InvalidOperationException();
        }

        public void Process(ICommunicationServer cs, int id)
        {
            cs.Send(this, GameId);
        }

        public abstract ActionInfo GetActionInfo();

        public virtual string ToLog()
        {
            return string.Join(',', PlayerGuid, GameId);
        }
    }
}