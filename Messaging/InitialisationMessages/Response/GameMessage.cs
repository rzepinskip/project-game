using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Common;
using Common.BoardObjects;
using Common.Interfaces;
using Messaging.Responses;

namespace Messaging.InitialisationMessages
{
    /// <summary>
    /// Game start messages sent to every player 
    /// </summary>
    [XmlType(XmlRootName)]
    public class GameMessage : Response
    {
        public const string XmlRootName = "Game";

        protected GameMessage()
        {
        }

        public GameMessage(int playerId, IEnumerable<PlayerBase> players, Location playerLocation, BoardInfo board) :
            base(playerId)
        {
            //Players = players.ToArray();
            PlayerLocation = playerLocation;
            Board = board;
        }

        //public PlayerBase[] Players { get; set; }
        public Location PlayerLocation { get; set; }
        public BoardInfo Board { get; set; }

        public override IMessage Process(IGameMaster gameMaster)
        {
            throw new NotImplementedException();
        }

        public override void Process(IPlayer player)
        {
            player.UpdatePlayerGame(PlayerLocation, Board);
        }

        public override void Process(ICommunicationServer cs, int id)
        {
            cs.UnregisterGame(id);
            cs.Send(this, PlayerId);
        }
    }
}