using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Common;
using Common.BoardObjects;
using Common.Interfaces;

namespace Messaging.Responses
{
    [XmlType(XmlRootName)]
    public class ResponseWithData : Response
    {
        public const string XmlRootName = "Data";

        protected ResponseWithData()
        {
            TaskFields = new TaskField[0];
            GoalFields = new GoalField[0];
            Pieces = new Piece[0];
        }

        public ResponseWithData(int playerId, Location location, IEnumerable<TaskField> taskFields = null,
            IEnumerable<GoalField> goalFields = null, IEnumerable<Piece> pieces = null,
            bool gameFinished = false) : base(playerId)
        {
            PlayerLocation = location;
            TaskFields = taskFields?.ToArray();
            GoalFields = goalFields?.ToArray();
            Pieces = pieces?.ToArray();
            GameFinished = gameFinished;
        }

        public TaskField[] TaskFields { get; set; }

        public GoalField[] GoalFields { get; set; }

        public Piece[] Pieces { get; set; }

        public Location PlayerLocation { get; set; }

        [XmlAttribute("gameFinished")] public bool GameFinished { get; set; }

        public override IMessage Process(IGameMaster gameMaster)
        {
            throw new InvalidOperationException();
        }

        public override void Process(IPlayer player)
        {
            foreach (var taskField in TaskFields) player.Board.HandleTaskField(PlayerId, taskField);

            foreach (var goalField in GoalFields) player.Board.HandleGoalField(PlayerId, goalField);

            foreach (var piece in Pieces) player.Board.HandlePiece(PlayerId, piece);

            if (PlayerLocation != null)
                player.Board.HandlePlayerLocation(PlayerId, PlayerLocation);
        }

        public static IMessage ConvertToData(DataFieldSet datafieldset, bool isGameFinished)
        {
            return new ResponseWithData(datafieldset.PlayerId, datafieldset.PlayerLocation, datafieldset.TaskFields,
                datafieldset.GoalFields, datafieldset.Pieces, isGameFinished);
        }

        public virtual string Serialize()
        {
            return XmlExtensions.SerializeToXml(this);
        }

        public static ResponseWithData DeserializeFrom(string xml)
        {
            return XmlExtensions.DeserializeFromXml<ResponseWithData>(xml);
        }
    }
}