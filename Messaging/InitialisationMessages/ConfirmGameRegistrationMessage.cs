using System;
using System.Xml.Serialization;
using Common.Interfaces;

namespace Messaging.InitialisationMessages
{
    [XmlType(XmlRootName)]
    public class ConfirmGameRegistrationMessage : Message
    {
        public const string XmlRootName = "ConfirmGameRegistration";

        public ConfirmGameRegistrationMessage() { }

        public ConfirmGameRegistrationMessage(int gameId)
        {
            GameId = gameId;
        }

        

        public int GameId { get; set; }
        public override IMessage Process(IGameMaster gameMaster)
        {
            throw new NotImplementedException();
        }

        public override void Process(IGameMaster gameMaster, int i)
        {
            gameMaster.SetGameId(GameId);
        }

        public override bool Process(IPlayer player)
        {
            return false;
        }

        public override void Process(ICommunicationServer cs, int id)
        {
            throw new NotImplementedException();
        }
    }
}
