using System;
using System.Xml.Serialization;
using Common.Interfaces;

namespace Messaging.Responses
{
    public abstract class Response : IResponse, ILoggable
    {
        protected Response()
        {
        }

        public Response(int playerId)
        {
            PlayerId = playerId;
        }

        public virtual string ToLog()
        {
            return string.Join(',', PlayerId);
        }

        [XmlAttribute("playerId")] public int PlayerId { get; set; }

        public virtual IMessage Process(IGameMaster gameMaster)
        {
            throw new NotImplementedException();
        }

        public virtual void Process(IPlayer player)
        {
            throw new NotImplementedException();
        }
    }
}