using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Common;
using Common.Interfaces;

namespace Messaging.InitialisationMessages
{
    [XmlType(XmlRootName)]
    public class RegisteredGamesMessage : IMessage
    {
        public const string XmlRootName = "RegisteredGames";

        public RegisteredGamesMessage() { }
        public RegisteredGamesMessage(IEnumerable<GameInfo> games)
        {
            Games = games.ToArray();
        }

        public GameInfo[] Games { get; set; }

        public IMessage Process(IGameMaster gameMaster)
        {
            throw new NotImplementedException();
        }

        public bool Process(IPlayer player)
        {
            throw new NotImplementedException();
        }

        public void Process(ICommunicationServer cs, int id)
        {
            throw new NotImplementedException();
        }
    }
}
