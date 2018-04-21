using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Common;
using Common.Interfaces;
using Messaging.Responses;

namespace Messaging.InitialisationMessages
{
    /// <summary>
    /// CS's response to player about listing all joinable games
    /// </summary>
    [XmlType(XmlRootName)]
    public class RegisteredGamesMessage : Response
    {
        public const string XmlRootName = "RegisteredGames";

        public RegisteredGamesMessage()
        {
        }

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
            player.UpdateGameState(Games);
            return true;
        }

        public override void Process(ICommunicationServer cs, int id)
        {
            throw new NotImplementedException();
        }
    }
}