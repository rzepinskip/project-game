using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Common;
using Common.BoardObjects;
using Common.Interfaces;
using Messaging.ActionHelpers;
using Messaging.Responses;

namespace Messaging.Requests
{
    [XmlRoot(Namespace = "https://se2.mini.pw.edu.pl/17-results/")]
    public class DiscoverRequest : Request
    {
        public DiscoverRequest(int playerId) : base(playerId)
        {
        }

        public override Response Execute(IGameMasterBoard board)
        {
            var player = board.Players[PlayerId];
            var taskFields = new List<TaskField>();
            var pieces = new List<Piece>();

            var downLeftCorner = new Location(Math.Max(player.Location.X - 1, 0), Math.Max(player.Location.Y - 1, 0));
            var upRightCorner = new Location(Math.Min(player.Location.X + 1, board.Width),
                Math.Min(player.Location.Y + 1, board.Height));

            for (var i = downLeftCorner.X; i < Math.Min(upRightCorner.X + 1, board.Width); i++)
            for (var j = downLeftCorner.Y; j < Math.Min(upRightCorner.Y + 1, board.Height); j++)
                if (board[new Location(i, j)] is TaskField taskfield)
                {
                    taskfield.DistanceToPiece = board.DistanceToPieceFrom(taskfield);
                    taskFields.Add(taskfield);

                    if (taskfield.PieceId.HasValue)
                    {
                        var piece = board.Pieces[taskfield.PieceId.Value];
                        pieces.Add(new Piece(piece.Id, PieceType.Unknown, piece.PlayerId));
                    }
                }

            var response = new DiscoverResponse(PlayerId, taskFields, pieces);

            return response;
        }

        public override double GetDelay(ActionCosts actionCosts)
        {
            return actionCosts.DiscoverDelay;
        }

        public override ActionLog ToLog(int playerId, PlayerInfo playerInfo)
        {
            return new ActionLog(playerId, GameId, PlayerGuid, playerInfo, ActionType.Discover);
        }
    }
}