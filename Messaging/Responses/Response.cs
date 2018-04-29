using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Common.Interfaces;

namespace Messaging.Responses
{
    public abstract class Response : Message, IResponse, ILoggable, IEquatable<Response>
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
        
        public override bool Equals(object obj)
        {
            return Equals(obj as Response);
        }

        public bool Equals(Response other)
        {
            return other != null &&
                   PlayerId == other.PlayerId;
        }

        public override int GetHashCode()
        {
            var hashCode = -1020949546;
            hashCode = hashCode * -1521134295 + PlayerId.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(Response response1, Response response2)
        {
            return EqualityComparer<Response>.Default.Equals(response1, response2);
        }

        public static bool operator !=(Response response1, Response response2)
        {
            return !(response1 == response2);
        }

        public override void Process(ICommunicationServer cs, int id)
        {
            cs.Send(this, PlayerId);
        }
    }
}