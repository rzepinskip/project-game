using System;
using System.Xml.Serialization;
using Common.Interfaces;
using Messaging.Responses;

namespace Messaging.InitialisationMessages
{
    /// <summary>
    /// CS response to GM registration
    /// </summary>
    [XmlType(XmlRootName)]
    public class ConfirmGameRegistrationMessage : Response
    {
        public const string XmlRootName = "ConfirmGameRegistration";

        protected ConfirmGameRegistrationMessage()
        {
        }

        public ConfirmGameRegistrationMessage(int gameId)
        {
            GameId = gameId;
        }

        [XmlAttribute("gameId")] public int GameId { get; set; }

        public override IMessage Process(IGameMaster gameMaster)
        {
            gameMaster.SetGameId(GameId);
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