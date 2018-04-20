using System;
using System.Xml.Serialization;
using Common.Interfaces;

namespace Messaging.InitialisationMessages
{
    [XmlType(XmlRootName)]
    public class ConfirmGameRegistrationMessage : IMessage
    {
        public const string XmlRootName = "ConfirmGameRegistration";

        public ConfirmGameRegistrationMessage() { }

        public ConfirmGameRegistrationMessage(int gameId)
        {
            GameId = gameId;
        }

        public int GameId { get; set; }
        public IMessage Process(IGameMaster gameMaster)
        {
            //add new handler for confirm game registration
            throw new NotImplementedException();
        }

        public bool Process(IPlayer player)
        {
            return false;
        }

        public void Process(ICommunicationServer cs, int id)
        {
            throw new NotImplementedException();
        }
    }
}
