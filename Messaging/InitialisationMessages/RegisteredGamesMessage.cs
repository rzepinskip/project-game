using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Common;
using Common.Interfaces;

namespace Messaging.InitialisationMessages
{
    [XmlType(XmlRootName)]
    public class RegisteredGamesMessage : Message
    {
        public const string XmlRootName = "RegisteredGames";

        public RegisteredGamesMessage() { }
        public RegisteredGamesMessage(IEnumerable<GameInfo> games)
        {
            Games = games.ToArray();
        }

        public GameInfo[] Games { get; set; }

        public override IMessage Process(IGameMaster gameMaster)
        {
            throw new NotImplementedException();
        }

        public override bool Process(IPlayer player)
        {
            throw new NotImplementedException();
        }

        public override void Process(ICommunicationServer cs, int id)
        {
            throw new NotImplementedException();
        }
    }
}
