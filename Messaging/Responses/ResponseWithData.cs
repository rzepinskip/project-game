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
    public class ResponseWithData : Response, IEquatable<ResponseWithData>
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
            if (TaskFields != null)
                foreach (var taskField in TaskFields)
                    player.Board.HandleTaskField(PlayerId, taskField);

            if (GoalFields != null)
                foreach (var goalField in GoalFields)
                    player.Board.HandleGoalField(PlayerId, goalField);

            if (Pieces != null)
                foreach (var piece in Pieces)
                    player.Board.HandlePiece(PlayerId, piece);

            if (PlayerLocation != null)
                player.Board.HandlePlayerLocation(PlayerId, PlayerLocation);
        }

        public static IMessage ConvertToData(DataFieldSet datafieldset, bool isGameFinished)
        {
            return new ResponseWithData(datafieldset.PlayerId, datafieldset.PlayerLocation, datafieldset.TaskFields,
                datafieldset.GoalFields, datafieldset.Pieces, isGameFinished);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ResponseWithData);
        }

        public bool Equals(ResponseWithData other)
        {
            return other != null &&
                   base.Equals(other) &&
                   TaskFields.SequenceEqual(other.TaskFields) &&
                   GoalFields.SequenceEqual(other.GoalFields) &&
                   Pieces.SequenceEqual(other.Pieces) &&
                   EqualityComparer<Location>.Default.Equals(PlayerLocation, other.PlayerLocation) &&
                   GameFinished == other.GameFinished;
        }

        public override int GetHashCode()
        {
            var hashCode = 1775193206;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<TaskField[]>.Default.GetHashCode(TaskFields);
            hashCode = hashCode * -1521134295 + EqualityComparer<GoalField[]>.Default.GetHashCode(GoalFields);
            hashCode = hashCode * -1521134295 + EqualityComparer<Piece[]>.Default.GetHashCode(Pieces);
            hashCode = hashCode * -1521134295 + EqualityComparer<Location>.Default.GetHashCode(PlayerLocation);
            hashCode = hashCode * -1521134295 + GameFinished.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(ResponseWithData data1, ResponseWithData data2)
        {
            return EqualityComparer<ResponseWithData>.Default.Equals(data1, data2);
        }

        public static bool operator !=(ResponseWithData data1, ResponseWithData data2)
        {
            return !(data1 == data2);
        }
    }
}