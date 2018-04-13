using System;
using System.Collections.Generic;
using System.Text;
using Common;
using Common.BoardObjects;
using Common.Interfaces;

namespace Messaging.InitialisationMessages
{
    public class GameMessage : IResponse
    {
        public int PlayerId { get; set; }
        public GameMessage(IEnumerable<PlayerBase> players, Location playerLocation, BoardInfo board)
        {
            this.Players = players;
            this.PlayerLocation = playerLocation;
            this.Board = board;
        }

        public IEnumerable<PlayerBase> Players { get; set; }
        public Location PlayerLocation { get; set; }
        public BoardInfo Board { get; set; }

        public IMessage Process(IGameMaster gameMaster)
        {
            throw new NotImplementedException();
        }

        public void Process(IPlayer player)
        {
            throw new NotImplementedException();
        }

        public void Process(ICommunicationServer cs, int id)
        {
            cs.Send(this, this.PlayerId);
        }

    }
}
