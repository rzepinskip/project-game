using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Common;
using Common.BoardObjects;
using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

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

       
        public void HandleTaskField(int playerId, TaskField taskField, ref List<Piece> pieces)
        {
            var oldTaskField = (TaskField)this[taskField];

            if (DateTime.Compare(oldTaskField.Timestamp, taskField.Timestamp) < 0)
            {
                ClearPlayerFromField(taskField);
                ClearPieceFromField(taskField);

                if (taskField.PlayerId.HasValue)
                {
                    HandlePlayerInField(taskField, pieces);
                }

                if (taskField.PieceId.HasValue)
                {
                    HandlePieceInField(taskField, pieces);
                }

                this[taskField] = new TaskField(taskField, taskField.DistanceToPiece, taskField.PieceId, taskField.PlayerId);
            }
        }

        public void HandleGoalField(int playerId, GoalField goalField, ref List<Piece> pieces)
        {
            var oldGoalField = this[goalField];
            if (IsNewer(goalField, oldGoalField))
            {
                ClearPlayerFromField(oldGoalField);

                if (goalField.PlayerId.HasValue)
                {
                    HandlePlayerInField(goalField, pieces);
                }

                if (goalField.PlayerId.HasValue)
                {
                    var player = Players[goalField.PlayerId.Value];
                    player.Location = new Location(goalField.X, goalField.Y);
                }

                this[goalField] = goalField;
            }
        }

        public void HandleGoalFieldAfterPlace(int playerId, GoalField goalField)
        {
            var playerInfo = Players[playerId];

            Pieces.Remove(playerInfo.Piece.Id);
            playerInfo.Piece = null;

            this[goalField] = goalField;
        }


        private bool IsNewer(Field filed, Field fieldToComapre)
        {
            return DateTime.Compare(filed.Timestamp, fieldToComapre.Timestamp) > 0;
        }

        private void ClearPlayerFromField(Field field)
        {
            if (field.PlayerId.HasValue)
            {
                var player = Players[field.PlayerId.Value];
                if (player.Piece != null)
                {
                    Pieces.Remove(player.Piece.Id);
                    player.Piece = null;
                }

                player.Location = null;
                field.PlayerId = null;
            }
        }

        private void ClearPieceFromField(TaskField taskField)
        {
            if (taskField.PieceId.HasValue)
            {
                Pieces.Remove(taskField.PieceId.Value);
                taskField.PieceId = null;
            }
        }

        private void HandlePlayerInField(Field field, List<Piece> pieces)
        {
            var player = Players[field.PlayerId.Value];
            if (player.Location == null)
            {
                player.Location = new Location(field.X, field.Y);
            }
            else
            {
                var oldPlayerField = this[player.Location];

                if (IsNewer(field, oldPlayerField))
                {
                    if (player.Piece != null)
                        Pieces.Remove(player.Piece.Id);

                    oldPlayerField.PlayerId = null;
                    player.Location = new Location(field.X, field.Y);
                }
                else
                {
                    foreach (var piece in pieces.ToList())
                    {
                        if (piece.PlayerId == player.Id)
                        {
                            pieces.Remove(piece);
                            break;
                        }
                    }

                    field.PlayerId = null;
                }
            }
        }

        private void HandlePieceInField(TaskField taskField, List<Piece> pieces)
        {
            var piece = Pieces[taskField.PieceId.Value];

            var oldPieceField = FindFieldWithPiece(piece.Id);
            if (oldPieceField != null)
            {
                if (IsNewer(taskField, oldPieceField))
                {
                    Pieces.Remove(oldPieceField.PieceId.Value);
                    oldPieceField.PieceId = null;
                }
                else
                {
                    pieces.Remove(piece);
                    taskField.PieceId = null;
                }
            }
        }

        private TaskField FindFieldWithPiece(int pieceId)
        {
            TaskField result = null;
            for (var i = 0; i < Width; ++i)
            {
                for (var j = GoalAreaSize; j < TaskAreaSize + GoalAreaSize; ++j)
                {
                    var field = Content[i, j];
                    if (!IsLocationInTaskArea(field)) continue;

                    var taskFiled = (TaskField)field;
                    if (taskFiled.PieceId != pieceId) continue;

                    result = taskFiled;
                    break;
                }
            }

            return result;
        }


        public void HandlePiece(int playerId, Piece piece)
        {
            if (Pieces.ContainsKey(piece.Id))
            {
                var oldPiece = Pieces[piece.Id];
                if (oldPiece.PlayerId.HasValue)
                    Players[oldPiece.PlayerId.Value].Piece = null;

                Pieces[piece.Id] = piece;
            }
            else
            {
                Pieces.Add(piece.Id, piece);
            }


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