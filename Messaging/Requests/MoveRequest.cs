using System.Collections.Generic;
using System.Xml.Serialization;
using Common.ActionAvailability.AvailabilityChain;
using Common.BoardObjects;
using Common.Interfaces;
using Messaging.ActionHelpers;
using Messaging.Responses;

namespace Common.GameMessages
{
    [XmlRoot(Namespace = "https://se2.mini.pw.edu.pl/17-results/")]
    public class MoveRequest : Request
    {
        [XmlAttribute] public bool DirectionFieldSpecified;

        [XmlAttribute]
        public Direction Direction { get; set; }

        public override Response Execute(IBoard board)
        {
            var player = board.Players[PlayerId];

            var taskFields = new List<TaskField>();
            var pieces = new List<Piece>();
            var response = new MoveResponse {PlayerId = PlayerId, TaskFields = taskFields, Pieces = pieces};

            var actionAvailability = new MoveAvailabilityChain(player.Location, Direction, player.Team, board);
            if (actionAvailability.ActionAvailable())
            {
                board[player.Location].PlayerId = null;
                var newLocation = player.Location.GetNewLocation(Direction);
                var field = board[newLocation];
                field.PlayerId = PlayerId;
                player.Location = newLocation;

                response.NewPlayerLocation = newLocation;
                if (field is TaskField taskField)
                {
                    taskField.DistanceToPiece = board.DistanceToPieceFrom(taskField);
                    taskFields.Add(taskField);

                    if (taskField.PieceId.HasValue)
                    {
                        var piece = board.Pieces[taskField.PieceId.Value];
                        pieces.Add(new Piece(piece.Id, PieceType.Unknown, piece.PlayerId));
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