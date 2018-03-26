using System.Xml.Serialization;
using Common;
using Common.ActionInfo;
using Common.Interfaces;

namespace Messaging.Requests
{
    [XmlRoot(Namespace = "https://se2.mini.pw.edu.pl/17-results/")]
    public class DiscoverRequest : Request, ILoggable
    {
        public DiscoverRequest(string playerGuid) : base(playerGuid)
        {
        }

        /*
        //TODO: move to GameMaster.ExecuteAction(DiscoverActionInfo dai, bool gameFinished)
        public override Response Execute(IBoard board)
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
        */

        public override string ToLog()
        {
            return string.Join(',', ActionType.Discover, base.ToLog());
        }

        public override ActionInfo GetActionInfo()
        {
            return new DiscoverActionInfo(PlayerGuid);
        }
    }
}