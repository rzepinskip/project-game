using Common;
using Common.BoardObjects;
using Common.Interfaces;

namespace Player
{
    public class PlayerBoard : BoardBase, IPlayerBoard
    {
        public PlayerBoard(int boardWidth, int taskAreaSize, int goalAreaSize) : base(boardWidth, taskAreaSize,
            goalAreaSize)
        {
        }

        public void HandlePlayerLocation(int playerId, Location playerUpdatedLocation)
        {
            // Remove old data
            var playerInfo = Players[playerId];
            this[playerInfo.Location].PlayerId = null;

            // Insert new data
            playerInfo.Location = playerUpdatedLocation;
            this[playerUpdatedLocation].PlayerId = playerId;
        }

        public void HandlePiece(int playerId, Piece piece)
        {
            // Insert new data
            if (Pieces.ContainsKey(piece.Id))
                Pieces[piece.Id] = piece;
            else
                Pieces.Add(piece.Id, piece);

            if (piece.PlayerId.HasValue)
            {
                Players[piece.PlayerId.Value].Piece = piece;
            }

            if (piece.Type == PieceType.Sham)
            {
                Pieces.Remove(piece.Id);
                if (piece.PlayerId.HasValue)
                {
                    Players[piece.PlayerId.Value].Piece = null;
                }
            }
        }

        public void HandleTaskField(int playerId, TaskField taskField)
        {
            // Remove old data
            //var oldPlayer = this[taskField].PlayerId;
            //if (oldPlayer.HasValue)
            //{
            //    Players[oldPlayer.Value].Location = null;
            //}

            //var oldPiece = (this[taskField] as TaskField).PieceId;
            //if (oldPiece.HasValue)
            //{
            //    Pieces.Remove(oldPiece.Value);
            //}

            // Insert new data
            this[taskField] = new TaskField(taskField, taskField.DistanceToPiece, taskField.PieceId, taskField.PlayerId);
        }

        public void HandleGoalField(int playerId, GoalField goalField)
        {
            // Remove old data
            foreach (var piecesValue in Pieces.Values)
            {
                if (piecesValue.PlayerId == playerId)
                {
                    piecesValue.PlayerId = null;
                    break;
                }
            }

            // Insert new data
            this[goalField] = goalField;
        }
    }
}