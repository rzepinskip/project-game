using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Common;
using Common.BoardObjects;
using Common.Interfaces;
using Messaging.KnowledgeExchangeMessages;

namespace Messaging
{
    [XmlType(XmlRootName)]
    public class DataMessage : MessageToPlayer, IEquatable<DataMessage>
    {
        public const string XmlRootName = "Data";

        protected DataMessage()
        {
            TaskFields = new TaskField[0];
            GoalFields = new GoalField[0];
            Pieces = new Piece[0];
        }

        public DataMessage(int playerId, Location location, IEnumerable<TaskField> taskFields = null,
            IEnumerable<GoalField> goalFields = null, IEnumerable<Piece> pieces = null,
            bool gameFinished = false, Guid? senderGuid = null) : base(playerId)
        {
            PlayerLocation = location;
            TaskFields = taskFields?.ToArray();
            GoalFields = goalFields?.ToArray();
            Pieces = pieces?.ToArray();
            GameFinished = gameFinished;
            PlayerGuid = senderGuid;
        }

        public TaskField[] TaskFields { get; set; }

        public GoalField[] GoalFields { get; set; }

        public Piece[] Pieces { get; set; }

        public Location PlayerLocation { get; set; }

        [XmlAttribute("gameFinished")] public bool GameFinished { get; set; }

        [XmlIgnore] public Guid? PlayerGuid { get; set; }

        [XmlAttribute("playerGuid")]
        protected Guid PlayerGuidValue
        {
            get
            {
                if (PlayerGuid != null) return PlayerGuid.Value;

                throw new InvalidOperationException();
            }
            set => PlayerGuid = value;
        }

        [XmlIgnore] protected bool PlayerGuidValueSpecified => PlayerGuid.HasValue;

        public override IMessage Process(IGameMaster gameMaster)
        {
            var knowledgeExchangeManager = gameMaster.KnowledgeExchangeManager;
            if (PlayerGuid == null) return null;
            var playerGuidValue = PlayerGuid.Value;
            var optionalSenderId = gameMaster.Authorize(playerGuidValue);
            if (!optionalSenderId.HasValue) return null;
            var senderId = optionalSenderId.Value;
            // stripping guid from data;
            PlayerGuid = null;
            if (knowledgeExchangeManager.IsExchangeInitiator(senderId, PlayerId))
            {
                knowledgeExchangeManager.AttachDataToInitiator(this, senderId, PlayerId);
                //send KnowledgeExchangeRequest to PlayerId
                return new KnowledgeExchangeRequestMessage(PlayerId, senderId);
            }

            if (knowledgeExchangeManager.HasMatchingInitiatorWithData(senderId, PlayerId))
            {
                gameMaster.SendDataToInitiator(PlayerId, this);
                return knowledgeExchangeManager.FinalizeExchange(PlayerId, senderId);
            }
            // No matching knowledge exchange initiator

            return null;
        }

        public override void Process(IPlayer player)
        {
            var pieces = Pieces.ToList();

            if (GoalFields.Length == 1)
            {
                player.Board.HandleGoalFieldAfterPlace(PlayerId, GoalFields[0]);
            }
            else
            {
                foreach (var goalField in GoalFields)
                    player.Board.HandleGoalField(PlayerId, goalField, ref pieces);
            }

            foreach (var taskField in TaskFields)
                player.Board.HandleTaskField(PlayerId, taskField, ref pieces);

            foreach (var piece in pieces)
                player.Board.HandlePiece(PlayerId, piece);

            if (PlayerLocation != null)
                player.Board.HandlePlayerLocation(PlayerId, PlayerLocation);

            if (GameFinished)
                player.NotifyAboutGameEnd();
        }

        public static IMessage FromBoardData(BoardData boardData, bool isGameFinished, Guid? PlayerGuid = null)
        {
            if (boardData == null)
                return null;

            return new DataMessage(boardData.PlayerId, boardData.PlayerLocation, boardData.TaskFields,
                    boardData.GoalFields, boardData.Pieces, isGameFinished);
        }

        #region Equality
        public bool Equals(DataMessage other)
        {
            return other != null &&
                   base.Equals(other) &&
                   IsCollectionsEqual(TaskFields, other.TaskFields) &&
                   IsCollectionsEqual(GoalFields, other.GoalFields) &&
                   IsCollectionsEqual(Pieces, other.Pieces) &&
                   EqualityComparer<Location>.Default.Equals(PlayerLocation, other.PlayerLocation) &&
                   GameFinished == other.GameFinished;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as DataMessage);
        }

        private bool IsCollectionsEqual<T>(IEnumerable<T> l1, IEnumerable<T> l2)
        {
            if (l1 == null && l2 == null)
                return true;

            if (l1 == null || l2 == null)
                return false;

            if (l1.Any(item => !l2.Contains(item)))
                return false;

            if (l2.Any(item => !l1.Contains(item)))
                return false;

            return true;
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

        public static bool operator ==(DataMessage data1, DataMessage data2)
        {
            return EqualityComparer<DataMessage>.Default.Equals(data1, data2);
        }

        public static bool operator !=(DataMessage data1, DataMessage data2)
        {
            return !(data1 == data2);
        }
        #endregion
    }
}