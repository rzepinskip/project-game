using System.Collections.Generic;
using Common;
using Common.ActionAvailability.AvailabilityChain;
using Common.BoardObjects;

namespace GameMaster.ActionHandlers
{
    internal class MoveActionHandler : ActionHandler
    {
        private readonly Direction _direction;

        public MoveActionHandler(int playerId, GameMasterBoard board, Direction direction) : base(playerId, board)
        {
            _direction = direction;
        }

        protected override bool Validate()
        {
            var playerInfo = Board.Players[PlayerId];

            var actionAvailability = new MoveAvailabilityChain(playerInfo.Location, _direction, playerInfo.Team, Board);
            return actionAvailability.ActionAvailable();
        }

        public override DataFieldSet Respond()
        {
            var player = Board.Players[PlayerId];
            var taskFields = new List<TaskField>();
            var pieces = new List<Piece>();

            Location newPlayerLocation;
            if (Validate())
            {
                Board[player.Location].PlayerId = null;
                var newLocation = player.Location.GetNewLocation(_direction);
                var field = Board[newLocation];
                field.PlayerId = PlayerId;
                player.Location = newLocation;

                newPlayerLocation = newLocation;
                if (field is TaskField taskField)
                {
                    taskField.DistanceToPiece = Board.DistanceToPieceFrom(taskField);
                    taskFields.Add(taskField);

                    if (taskField.PieceId.HasValue)
                    {
                        var piece = Board.Pieces[taskField.PieceId.Value];
                        pieces.Add(new Piece(piece.Id, PieceType.Unknown, piece.PlayerId));
                    }
                }
            }
            else
            {
                newPlayerLocation = player.Location;
            }

            return DataFieldSet.Create(PlayerId, taskFields.ToArray(), pieces.ToArray(), newPlayerLocation);
        }
    }
}