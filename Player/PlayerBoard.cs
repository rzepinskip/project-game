using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Common;
using Common.BoardObjects;
using Common.Interfaces;

namespace Player
{
    public class PlayerBoard : BoardBase, IPlayerBoard
    {
        protected PlayerBoard()
        {
        }

        public PlayerBoard(int boardWidth, int taskAreaSize, int goalAreaSize) : base(boardWidth, taskAreaSize,
            goalAreaSize)
        {
        }

        public void HandleTaskField(int playerId, TaskField taskField)
        {
            // Remove old data
            //var oldPlayer = this[taskField].PlayerId;
            //if (oldPlayer.HasValue)
            //{
            //    Players[oldPlayer.Value].Location = null;
            //}

            //var oldPiece = (this[taskField] as TaskField).PieceId;
            //if (oldPiece.HasValue)
            //{
            //    Pieces.Remove(oldPiece.Value);
            //}

            // Insert new data
            this[taskField] =
                new TaskField(taskField, taskField.DistanceToPiece, taskField.PieceId, taskField.PlayerId);

            if (taskField.PlayerId.HasValue)
                Players[taskField.PlayerId.Value].Location = new Location(taskField.X, taskField.Y);
        }

        public void HandleGoalField(int playerId, GoalField goalField)
        {
            //// Remove old data
            //foreach (var piecesValue in Pieces.Values)
            //    if (piecesValue.PlayerId == playerId)
            //    {
            //        piecesValue.PlayerId = null;
            //        break;
            //    }

            var playerInfo = Players[playerId];
            var pieceId = playerInfo.Piece.Id;

            playerInfo.Piece = null;
            Pieces.Remove(pieceId);

            // Insert new data
            this[goalField] = goalField;
        }

        public void HandlePiece(int playerId, Piece piece)
        {
            // Insert new data
            if (Pieces.ContainsKey(piece.Id))
                Pieces[piece.Id] = piece;
            else
                Pieces.Add(piece.Id, piece);

            if (piece.PlayerId.HasValue) Players[piece.PlayerId.Value].Piece = piece;

            if (piece.Type == PieceType.Destroyed)
            {
                Pieces.Remove(piece.Id);
                if (piece.PlayerId.HasValue) Players[piece.PlayerId.Value].Piece = null;
            }
        }

        public void HandlePlayerLocation(int playerId, Location playerUpdatedLocation)
        {
            // Remove old data
            var playerInfo = Players[playerId];
            this[playerInfo.Location].PlayerId = null;

            // Insert new data
            playerInfo.Location = playerUpdatedLocation;
            this[playerUpdatedLocation].PlayerId = playerId;
        }

        public override void ReadXml(XmlReader reader)
        {
            base.ReadXml(reader);

            reader.ReadEndElement();
        }

        public BoardData ToBoardData(int responderId, int dataReceiverId)
        {
            var taskFields = ToEnumerable().Where(f => f is TaskField taskField && taskField.DistanceToPiece != -1).Select(t => (TaskField)t);
            var goalFields = ToEnumerable().Where(f => f is GoalField goalField && goalField.Type != GoalFieldType.Unknown).Select(t => (GoalField)t);
            var pieces = Pieces.Values.ToArray();

            return BoardData.Create(dataReceiverId, Players[responderId].Location, taskFields.ToArray(), goalFields.ToArray(), pieces);
        }
    }
}