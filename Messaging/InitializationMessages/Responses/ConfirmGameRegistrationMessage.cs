using System;
using System.Xml.Serialization;
using Common.Interfaces;

namespace Messaging.InitializationMessages
{
    /// <summary>
    ///     CS response to GM registration
    /// </summary>
    [XmlType(XmlRootName)]
    public class ConfirmGameRegistrationMessage : Message
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

        public override string ToLog()
        {
            return string.Join(',', XmlRootName, GameId);
        }
    }
}