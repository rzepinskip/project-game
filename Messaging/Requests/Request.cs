using System;
using System.Xml.Serialization;
using Common.ActionInfo;
using Common.Interfaces;

namespace Messaging.Requests
{
    public abstract class Request : IRequest
    {
        public Request(string playerGuid)
        {
            PlayerGuid = playerGuid;
        }

        [XmlAttribute] public int GameId { get; set; }

        [XmlAttribute] public string PlayerGuid { get; set; }

        public IMessage Process(IGameMaster gameMaster)
        {
            var result = gameMaster.EvaluateAction(GetActionInfo());
            return ResponseWithData.ConvertToData(result.data, result.isGameFinished);
        }

        public void Process(IPlayer player)
        {
            throw new InvalidOperationException();
        }

        public abstract ActionInfo GetActionInfo();

        public virtual string ToLog()
        {
            return string.Join(',', PlayerGuid, GameId);
        }
    }
}