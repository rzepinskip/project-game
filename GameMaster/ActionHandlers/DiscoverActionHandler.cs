using System;
using System.Collections.Generic;
using Common;
using Common.BoardObjects;

namespace GameMaster.ActionHandlers
{
    internal class DiscoverActionHandler : ActionHandler
    {
        public DiscoverActionHandler(int playerId, GameMasterBoard board) : base(playerId, board)
        {
            PlayerId = playerId;
            Board = board;
        }

        protected override bool Validate()
        {
            return true;
        }

        public override DataFieldSet Respond()
        {
            var player = Board.Players[PlayerId];
            var taskFields = new List<TaskField>();
            var pieces = new List<Piece>();

            var downLeftCorner = new Location(Math.Max(player.Location.X - 1, 0), Math.Max(player.Location.Y - 1, 0));
            var upRightCorner = new Location(Math.Min(player.Location.X + 1, Board.Width),
                Math.Min(player.Location.Y + 1, Board.Height));

            for (var i = downLeftCorner.X; i < Math.Min(upRightCorner.X + 1, Board.Width); i++)
            for (var j = downLeftCorner.Y; j < Math.Min(upRightCorner.Y + 1, Board.Height); j++)
                if (Board[new Location(i, j)] is TaskField taskfield)
                {
                    taskfield.DistanceToPiece = Board.DistanceToPieceFrom(taskfield);
                    taskFields.Add(taskfield);

                    if (taskfield.PieceId.HasValue)
                    {
                        var piece = Board.Pieces[taskfield.PieceId.Value];
                        pieces.Add(new Piece(piece.Id, PieceType.Unknown, piece.PlayerId));
                    }

                    if (taskfield.PlayerId.HasValue && Board.Players[taskfield.PlayerId.Value].Piece != null)
                    {
                        var piece = Board.Players[taskfield.PlayerId.Value].Piece;
                        pieces.Add(new Piece(piece.Id, PieceType.Unknown, piece.PlayerId));
                    }
                }

            return DataFieldSet.Create(PlayerId, taskFields.ToArray(), pieces.ToArray());
        }
    }
}