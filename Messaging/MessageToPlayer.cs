using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Common.Interfaces;

namespace Messaging
{
    public abstract class MessageToPlayer : Message, IResponseMessage, IEquatable<MessageToPlayer>
    {
        protected MessageToPlayer()
        {
        }

        protected MessageToPlayer(int playerId)
        {
            PlayerId = playerId;
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

        public override void Process(ICommunicationServer cs, int id)
        {
            cs.Send(this, PlayerId);
        }

        public override string ToLog()
        {
            return PlayerId.ToString();
        }

        #region Equality
        public bool Equals(MessageToPlayer other)
        {
            return other != null &&
                   PlayerId == other.PlayerId;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as MessageToPlayer);
        }

        public override int GetHashCode()
        {
            var hashCode = -1020949546;
            hashCode = hashCode * -1521134295 + PlayerId.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(MessageToPlayer response1, MessageToPlayer response2)
        {
            return EqualityComparer<MessageToPlayer>.Default.Equals(response1, response2);
        }

        public static bool operator !=(MessageToPlayer response1, MessageToPlayer response2)
        {
            return !(response1 == response2);
        }
        #endregion
    }
}