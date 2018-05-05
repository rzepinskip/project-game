using System.Collections.Generic;
using ClientsCommon.ActionAvailability.AvailabilityChain;
using Common;
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
            var newLocation = player.Location.GetNewLocation(_direction);
            var fieldAtNewLocation = Board[newLocation];

            if (fieldAtNewLocation is TaskField taskField)
            {
                taskField.DistanceToPiece = Board.DistanceToPieceFrom(taskField);
                taskFields.Add(taskField.Clone());

                if (taskField.PieceId.HasValue)
                {
                    var piece = Board.Pieces[taskField.PieceId.Value];
                    pieces.Add(new Piece(piece.Id, PieceType.Unknown, piece.PlayerId));
                }
            }

            if (Validate())
            {
                Board[player.Location].PlayerId = null;
                fieldAtNewLocation.PlayerId = PlayerId;
                player.Location = newLocation;

                newPlayerLocation = newLocation;
            }
            else
            {
                newPlayerLocation = player.Location;

                if (fieldAtNewLocation.PlayerId.HasValue && Board.Players[fieldAtNewLocation.PlayerId.Value].Piece != null)
                {
                    var piece = Board.Players[fieldAtNewLocation.PlayerId.Value].Piece;
                    pieces.Add(new Piece(piece.Id, PieceType.Unknown, piece.PlayerId));
                }
            }

            return DataFieldSet.Create(PlayerId, newPlayerLocation, taskFields.ToArray(), pieces.ToArray());
        }
    }
}