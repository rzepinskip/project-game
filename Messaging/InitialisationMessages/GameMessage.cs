using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Common;
using Common.BoardObjects;
using Common.Interfaces;
using Messaging.Responses;

namespace Messaging.InitialisationMessages
{
    [XmlType(XmlRootName)]
    public class GameMessage : Response
    {
        public const string XmlRootName = "Game";

        public GameMessage() { }
        public GameMessage(IEnumerable<PlayerBase> players, Location playerLocation, BoardInfo board)
        {
            Players = players.ToArray();
            PlayerLocation = playerLocation;
            Board = board;
        }

        public PlayerBase[] Players { get; set; }
        public Location PlayerLocation { get; set; }
        public BoardInfo Board { get; set; }

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
            cs.UnregisterGame(id);
            cs.Send(this, PlayerId);
        }

    }
}
