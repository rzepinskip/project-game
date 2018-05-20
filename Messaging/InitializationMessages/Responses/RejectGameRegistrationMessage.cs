using System;
using System.Xml.Serialization;
using Common;
using Common.Interfaces;

namespace Messaging.InitializationMessages
{
    [XmlType(XmlRootName)]
    public class RejectGameRegistrationMessage : MessageToPlayer
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
            throw new ApplicationFatalException("Failed to register game");
        }

        public override void Process(IPlayer player)
        {
            throw new NotImplementedException();
        }

        public override void Process(ICommunicationServer cs, int id)
        {
            throw new NotImplementedException();
        }

        public override string ToLog()
        {
            return $"{XmlRootName} in {GameName}";
        }
    }
}