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
        private bool IsStepInsideBoard()
        {
            var playerInfo = Board.Players[PlayerId];

            var actionAvailability = new StepInsideBoard(playerInfo.Location, _direction, playerInfo.Team, Board);
            return actionAvailability.ActionAvailable();
        }

        public override BoardData Respond()
        {
            var player = Board.Players[PlayerId];

            if (!IsStepInsideBoard())
                return BoardData.Create(PlayerId, player.Location, new TaskField[0], new Piece[0]);

            var taskFields = new List<TaskField>();
            var pieces = new List<Piece>();

            Location resultPlayerLocation;
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
                resultPlayerLocation = newLocation;

                Board[player.Location].PlayerId = null;
                fieldAtNewLocation.PlayerId = PlayerId;
                player.Location = newLocation;

            }
            else
            {
                resultPlayerLocation = player.Location;

                if (fieldAtNewLocation.PlayerId.HasValue &&
                    Board.Players[fieldAtNewLocation.PlayerId.Value].Piece != null)
                {
                    var piece = Board.Players[fieldAtNewLocation.PlayerId.Value].Piece;
                    pieces.Add(new Piece(piece.Id, PieceType.Unknown, piece.PlayerId));
                }
            }

            return BoardData.Create(PlayerId, resultPlayerLocation, taskFields.ToArray(), pieces.ToArray());
        }
    }
}