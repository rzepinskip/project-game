using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Common;
using Common.BoardObjects;
using Common.Interfaces;

namespace Messaging.InitialisationMessages
{
    [XmlType(XmlRootName)]
    public class GameMessage : IResponse
    {
        public const string XmlRootName = "Game";
        public int PlayerId { get; set; }

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
            cs.UnregisterGame(id);
            cs.Send(this, PlayerId);
        }

    }
}
