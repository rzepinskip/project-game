using System.Collections.Generic;
using System.Xml.Serialization;
using Common;
using Common.ActionAvailability.AvailabilityChain;
using Common.ActionAvailability.AvailabilityLink;
using Common.BoardObjects;
using Common.Interfaces;
using Messaging.ActionHelpers;
using Messaging.Responses;

namespace Messaging.Requests
{
    [XmlRoot(Namespace = "https://se2.mini.pw.edu.pl/17-results/")]
    public class MoveRequest : Request
    {
        public MoveRequest(int playerId, Direction direction) : base(playerId)
        {
            Direction = direction;
        }

        [XmlAttribute]
        public Direction Direction { get; }

        public override Response Execute(IBoard board)
        {
            var player = board.Players[PlayerId];

            var taskFields = new List<TaskField>();
            var pieces = new List<Piece>();
            var newLocation = player.Location.GetNewLocation(Direction);

            var actionAvailability = new MoveAvailabilityChain(player.Location, Direction, player.Team, board);
            if (actionAvailability.ActionAvailable())
            {
                board[player.Location].PlayerId = null;
                var field = board[newLocation];
                field.PlayerId = PlayerId;
                player.Location = newLocation;

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
                var stepActionAvailability = new StepOnPlayerAvailabilityChain(player.Location, Direction, player.Team, board);
                if (stepActionAvailability.ActionAvailable())
                    taskFields.Add((TaskField)board[newLocation]);

                newLocation = player.Location;
            }

            var response = new MoveResponse(PlayerId, newLocation, taskFields, pieces);

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