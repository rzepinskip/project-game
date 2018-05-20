using System;
using ClientsCommon.ActionAvailability.AvailabilityChain;
using Common;
using Common.BoardObjects;

namespace GameMaster.ActionHandlers
{
    internal class PlacePieceActionHandler : ActionHandler
    {
        public PlacePieceActionHandler(int playerId, GameMasterBoard board) : base(playerId, board)
        {
            PlayerId = playerId;
            Board = board;
        }

        protected override bool Validate()
        {
            var playerInfo = Board.Players[PlayerId];
            return new PlaceAvailabilityChain(playerInfo.Location, Board, PlayerId).ActionAvailable();
        }

        private bool HavePiece()
        {
            var playerInfo = Board.Players[PlayerId];
            return new TryPlaceAvailabilityChain(playerInfo.Location, Board, PlayerId).ActionAvailable();
        }

        public override BoardData Respond()
        {
            if (!HavePiece())
                return BoardData.Create(PlayerId, new GoalField[0]);

            var player = Board.Players[PlayerId];
            var piece = Board.Pieces[player.Piece.Id];

            if (Validate())
            {

                player.Piece = null;

                if (Board.IsLocationInTaskArea(player.Location))
                {
                    piece.PlayerId = null;
                    

                    var playerGoalField = Board[player.Location] as TaskField;
                    playerGoalField.PieceId = piece.Id;

                    var resultPiece = new Piece(piece.Id, PieceType.Unknown);

                    return BoardData.Create(PlayerId, new[] { playerGoalField }, new[] { resultPiece });

                }
                else
                {
                    Board.Pieces.Remove(piece.Id);

                    if (piece.Type == PieceType.Sham)
                        return null;

                    var playerGoalField = Board[player.Location] as GoalField;

                    if (playerGoalField != null && playerGoalField.Type == GoalFieldType.Goal)
                        Board.MarkGoalAsCompleted(playerGoalField);

                    return BoardData.Create(PlayerId, new[] { playerGoalField });
                }
            }
            else
            {
                var playerTaskField = Board[player.Location] as TaskField;

                var pieceInField = Board.Pieces[playerTaskField.PieceId.Value];
                var resultPiece = new Piece(pieceInField.Id, PieceType.Unknown);
                var holdingPiece = new Piece(piece.Id, PieceType.Unknown, piece.PlayerId);


                return BoardData.Create(PlayerId, new[] { playerTaskField.Clone() }, new[] { resultPiece, holdingPiece });
            }
        }
    }
}