using System.Collections.Generic;
using System.Xml.Serialization;
using Shared.ActionAvailability.AvailabilityChain;
using Shared.BoardObjects;
using Shared.ResponseMessages;
using static Shared.CommonResources;

namespace Shared.GameMessages
{
    [XmlRoot(Namespace = "https://se2.mini.pw.edu.pl/17-results/")]
    public class Move : GameMessage
    {
        [XmlAttribute] public bool DirectionFieldSpecified;

        [XmlAttribute] public MoveType Direction { get; set; }

        public override ResponseMessage Execute(Board board)
        {
            var player = board.Players[PlayerId];

            var taskFields = new List<TaskField>();
            var pieces = new List<Piece>();
            var response = new MoveResponse {PlayerId = PlayerId, TaskFields = taskFields, Pieces = pieces};

            var actionAvailability = new MoveAvailabilityChain(player.Location, Direction, player.Team, board);
            if (actionAvailability.ActionAvailable())
            {
                board.Content[player.Location.X, player.Location.Y].PlayerId = null;
                var newLocation = player.Location.GetNewLocation(Direction);
                var field = board.Content[newLocation.X, newLocation.Y];
                field.PlayerId = PlayerId;
                player.Location = newLocation;

                response.NewPlayerLocation = newLocation;
                if (field is TaskField taskField)
                {
                    taskField.DistanceToPiece = board.GetDistanceToPiece(taskField);
                    taskFields.Add(taskField);

                    if (taskField.PieceId.HasValue)
                    {
                        var piece = board.Pieces[taskField.PieceId.Value];
                        pieces.Add(new Piece {Id = piece.Id, PlayerId = piece.PlayerId, Type = PieceType.Unknown});
                    }
                }
            }
            else
            {
                response.NewPlayerLocation = player.Location;
            }

            return response;
        }

        public override ActionLog ToLog(int playerId, PlayerInfo playerInfo)
        {
            return new ActionLog(playerId, GameId, PlayerGuid, playerInfo, ActionType.Move);
        }

        public override double GetDelay(ActionCosts actionCosts)
        {
            return actionCosts.MoveDelay;
        }
    }
}