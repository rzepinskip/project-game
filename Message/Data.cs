using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Common;
using Shared.BoardObjects;

namespace Message
{
    public class Data : Response
    {
        public Data(int playerId, Location location, IEnumerable<TaskField> taskFields = null,
            IEnumerable<GoalField> goalFields = null, IEnumerable<Piece> pieces = null,
            bool gameFinished = false) : base(playerId)
        {
            PlayerLocation = location;
            TaskFields = taskFields?.ToArray() ?? new TaskField[0];
            GoalFields = goalFields?.ToArray() ?? new GoalField[0];
            Pieces = pieces?.ToArray() ?? new Piece[0];
            GameFinished = gameFinished;
        }

        public TaskField[] TaskFields { get; set; }

        public GoalField[] GoalFields { get; set; }

        public Piece[] Pieces { get; set; }

        public Location PlayerLocation { get; set; }

        [XmlAttribute] public bool GameFinished { get; set; }

        public override IMessage Process(IGameMaster gameMaster)
        {
            throw new NotImplementedException();
        }

        public override void Process(IPlayer player)
        {
            player.Update(this);
        }
    }
}