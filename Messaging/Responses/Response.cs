using System;
using System.Xml.Serialization;
using Common.Interfaces;

namespace Messaging.Responses
{
    public abstract class Response : Message, IResponse, ILoggable
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

        public override IMessage Process(IGameMaster gameMaster)
        {
            throw new NotImplementedException();
        }

        public override void Process(IPlayer player)
        {
            throw new NotImplementedException();
        }

        public void Process(ICommunicationServer cs, int id)
        {
            cs.Send(this, this.PlayerId);
        }
    }
}