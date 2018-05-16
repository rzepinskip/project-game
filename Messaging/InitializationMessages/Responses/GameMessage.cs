using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Common;
using Common.BoardObjects;
using Common.Interfaces;

namespace Messaging.InitializationMessages
{
    /// <summary>
    ///     Game start messages sent to every player
    /// </summary>
    [XmlType(XmlRootName)]
    public class GameMessage : MessageToPlayer
    {
        public const string XmlRootName = "Game";

        protected GameMessage()
        {
        }

        public GameMessage(int playerId, IEnumerable<PlayerBase> players, Location playerLocation, BoardInfo board) :
            base(playerId)
        {
            Players = players.Select(p => new PlayerBase(p.Id, p.Team, p.Role)).ToArray();
            PlayerLocation = playerLocation;
            Board = board;
        }

        [XmlArray("Players")]
        [XmlArrayItem("Player")]
        public PlayerBase[] Players { get; set; }

        public Location PlayerLocation { get; set; }
        public BoardInfo Board { get; set; }

        public override IMessage Process(IGameMaster gameMaster)
        {
            throw new NotImplementedException();
        }

        public override void Process(IPlayer player)
        {
            player.InitializeGameData(PlayerLocation, Board, Players);
        }

        public override void Process(ICommunicationServer cs, int id)
        {
            cs.DeregisterGame(id);
            cs.Send(this, PlayerId);
        }

        public override string ToLog()
        {
            return string.Join(',', XmlRootName, base.ToLog());
        }
    }
}