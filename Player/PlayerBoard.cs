using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Common;
using Common.BoardObjects;
using Common.Interfaces;
using System;
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

        public void HandleTaskField(int playerId, TaskField taskField)
        {
            var oldTaskField = (TaskField)this[taskField];


            if (DateTime.Compare(oldTaskField.Timestamp, taskField.Timestamp) < 0)
            {
                if (oldTaskField.PlayerId.HasValue)
                {
                    Players[oldTaskField.PlayerId.Value].Location = null;
                }

                if (taskField.PlayerId.HasValue)
                {
                    var player = Players[taskField.PlayerId.Value];

                    if (player.Location != null)
                    {
                        var oldPlayerField = this[player.Location];

                        if (DateTime.Compare(oldPlayerField.Timestamp, taskField.Timestamp) < 0)
                        {
                            oldPlayerField.PlayerId = null;
                            player.Location = new Location(taskField.X, taskField.Y);
                        }
                        else
                        {
                            taskField.PlayerId = null; //!!!
                        }
                    }

                }


                if (oldTaskField.PieceId.HasValue)
                {
                    Pieces.Remove(oldTaskField.PieceId.Value);
                }

                if (taskField.PieceId.HasValue)
                {
                    var pieceId = taskField.PieceId.Value;

                    for (var i = 0; i < Width; ++i)
                    {
                        for (var j = GoalAreaSize; j < TaskAreaSize + GoalAreaSize; ++j)
                        {
                            var field = (TaskField)Content[i, j];
                            if (field.PieceId != pieceId)
                                continue;

                            if (DateTime.Compare(field.Timestamp, taskField.Timestamp) < 0)
                            {
                                field.PieceId = null;
                            }
                            else
                            {
                                taskField.PieceId = null; //!!!
                            }

                            break;
                        }
                    }
                }

                this[taskField] = new TaskField(taskField, taskField.DistanceToPiece, taskField.PieceId, taskField.PlayerId);
            }
        }

        public void HandleGoalField(int playerId, GoalField goalField)
        {
            var oldGoalField = this[goalField];

            if (DateTime.Compare(oldGoalField.Timestamp, goalField.Timestamp) < 0)
                this[goalField] = goalField;
        }

        public void HandleGoalFieldAfterPlace(int playerId, GoalField goalField)
        {
            var playerInfo = Players[playerId];
            var pieceId = playerInfo.Piece.Id;

            playerInfo.Piece = null;
            Pieces.Remove(pieceId);

            this[goalField] = goalField;
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