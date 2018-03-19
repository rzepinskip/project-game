using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Shared.BoardObjects;
using System.Xml.Serialization;
using Shared.ResponseMessages;
using static Shared.CommonResources;

namespace Shared.GameMessages
{
    [XmlRoot(Namespace = "https://se2.mini.pw.edu.pl/17-results/")]
    public class Discover : GameMessage
    {
        public override ResponseMessage Execute(Board board)
        {
            var player = board.Players[PlayerId];
            var taskFields = new List<TaskField>();
            var pieces = new List<Piece>();

            var response = new DiscoverResponse { PlayerId = PlayerId, TaskFields = taskFields, Pieces = pieces };

            var downLeftCorner = new Location { X = Math.Max(player.Location.X - 1, 0), Y = Math.Max(player.Location.Y - 1, 0) };
            var upRightCorner = new Location { X = Math.Min(player.Location.X + 1, board.Width), Y = Math.Min(player.Location.Y + 1, board.Height) };

            for (int i = downLeftCorner.X; i < Math.Min(upRightCorner.X + 1, board.Width); i++)
                for (int j = downLeftCorner.Y; j < Math.Min(upRightCorner.Y + 1, board.Height); j++)
                    if (board.Content[i, j] is TaskField taskfield)
                    {
                        taskfield.DistanceToPiece = board.GetDistanceToPiece(taskfield);
                        taskFields.Add(taskfield);

                        if (taskfield.PieceId.HasValue)
                        {
                            var piece = board.Pieces[taskfield.PieceId.Value];
                            pieces.Add(new Piece { Id = piece.Id, PlayerId = piece.PlayerId, Type = PieceType.Unknown });
                        }
                    }
            return response;
        }

        public override double GetDelay(ActionCosts actionCosts)
        {
            return actionCosts.DiscoverDelay;
        }

        public override ActionLog ToLog(int playerId, PlayerInfo playerInfo)
        {
            return new ActionLog(playerId, GameId, PlayerGuid, playerInfo, CommonResources.ActionType.Discover);
        }
    }
}
