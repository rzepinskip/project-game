using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Common;
using Common.BoardObjects;
using Common.Interfaces;

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

        [XmlAttribute]
        public bool GameFinished { get; set; }

        public override IMessage Process(IGameMaster gameMaster)
        {
            throw new NotImplementedException();
        }

        public override void Process(IPlayer player)
        {
            foreach (var taskField in TaskFields)
            {
                player.Board.HandleTaskField(taskField);
            }

            foreach (var goalField in GoalFields)
            {
                player.Board.HandleGoalField(goalField);
            }

            foreach (var piece in Pieces)
            {
                player.Board.HandlePiece(piece);
            }

            player.Board.HandlePlayerLocation(PlayerLocation);
        }
    }
}