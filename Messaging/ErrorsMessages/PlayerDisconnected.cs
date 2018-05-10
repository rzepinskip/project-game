using System;
using System.Xml.Serialization;
using Common.Interfaces;

namespace Messaging.ErrorsMessages
{
    [XmlType(XmlRootName)]
    public class PlayerDisconnected : Message
    {
        public const string XmlRootName = "PlayerDisconnected";

        protected PlayerDisconnected()
        {
        }

        public PlayerDisconnected(int playerId)
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
            throw new NotImplementedException();
        }
    }
}
