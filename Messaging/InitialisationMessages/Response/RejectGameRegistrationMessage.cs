using System;
using System.Xml.Serialization;
using Common.Interfaces;
using Messaging.Responses;

namespace Messaging.InitialisationMessages
{

    [XmlType(XmlRootName)]
    public class RejectGameRegistrationMessage : Message
    {
        public const string XmlRootName = "RejectGameRegistration";

        protected RejectGameRegistrationMessage()
        {
        }

        public RejectGameRegistrationMessage(string gameName)
        {
            GameName = gameName;
        }

        [XmlAttribute("gameName")] public string GameName { get; set; }

        public override IMessage Process(IGameMaster gameMaster)
        {
            gameMaster.RegisterGame();
            return null;
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
