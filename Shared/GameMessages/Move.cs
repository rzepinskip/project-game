using Shared.BoardObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Shared.ActionAvailability;
using Shared.ActionAvailability.AvailabilityChain;
using Shared.ActionAvailability.ActionAvailabilityHelpers;
using Shared.ResponseMessages;

namespace Shared.GameMessages
{

    [XmlRoot(Namespace = "https://se2.mini.pw.edu.pl/17-results/")]
    public class Move : GameMessage
    {
        [XmlAttribute()]
        public CommonResources.MoveType Direction { get; set; }

        [XmlAttribute()]
        public bool DirectionFieldSpecified;

        public override ResponseMessage Execute(Board board)
        {
            var player = board.Players[PlayerId];

            var response = new MoveResponse { PlayerId = PlayerId };
            var taskFields = new List<TaskField>();
            response.TaskFields = taskFields;

            var actionAvailability = new MoveAvailabilityChain(player.Location, Direction, player.Team, board);
            if (actionAvailability.ActionAvailable())
            {
                board.Content[player.Location.X, player.Location.Y].PlayerId = null;
                var newLocation = MoveAvailability.GetNewLocation(player.Location, Direction);

                var field = board.Content[newLocation.X, newLocation.Y];
                field.PlayerId = PlayerId;

                response.NewPlayerLocation = newLocation;
                if (board.IsLocationInTaskArea(newLocation))
                {
                     var taskField = (TaskField)field;
                    taskField.DistanceToPiece = board.GetManhattanDistance(taskField);
                    taskFields.Add(taskField);
                }
            }
            else
            {
                response.NewPlayerLocation = player.Location;
            }

            return response;
        }
    }
}
