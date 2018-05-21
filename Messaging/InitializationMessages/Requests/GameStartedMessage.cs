using System;
using System.Xml.Serialization;
using Common.Interfaces;

namespace Messaging.InitializationMessages
{
    /// <summary>
    ///     GM's request to deregister game - remove from publicly listed games
    /// </summary>
    [XmlType(XmlRootName)]
    public class GameStartedMessage : Message
    {
        public const string XmlRootName = "GameStarted";

        protected GameStartedMessage()
        {
        }

        public GameStartedMessage(int gameId)
        {
            GameId = gameId;
        }

        [XmlAttribute("gameId")] public int GameId { get; set; }

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
            throw new NotImplementedException("Should deregister game now");
        }

        public override string ToLog()
        {
            return string.Join(',', GameId, XmlRootName);
        }
    }
}