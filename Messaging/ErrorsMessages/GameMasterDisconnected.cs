using System;
using System.Xml.Serialization;
using Common.Interfaces;

namespace Messaging.ErrorsMessages
{
    [XmlType(XmlRootName)]
    public class GameMasterDisconnected : Message
    {
        public const string XmlRootName = "GameMasterDisconnected";

        protected GameMasterDisconnected()
        {
        }

        public GameMasterDisconnected(int gameId)
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
            player.HandleGameMasterDisconnection();
        }

        public override void Process(ICommunicationServer cs, int id)
        {
            throw new NotImplementedException();
        }
    }
}