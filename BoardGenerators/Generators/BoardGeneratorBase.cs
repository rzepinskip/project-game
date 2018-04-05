using System.Collections.Generic;
using System.IO;
using Common;
using Common.BoardObjects;
using Common.Interfaces;

namespace BoardGenerators.Generators
{
    public abstract class BoardGeneratorBase<TBoard> where TBoard : IBoard
    {
        protected abstract TBoard Board { get; set; }

        protected virtual void PlacePieces(IEnumerable<(Piece piece, Location location)> piecesWithLocations)
        {
            foreach (var (piece, location) in piecesWithLocations)
            {
                var taskFieldToFill = Board[location] as TaskField;
                if (taskFieldToFill == null)
                    throw new InvalidDataException();

                taskFieldToFill.DistanceToPiece = 0;
                taskFieldToFill.PieceId = piece.Id;

                Board.Pieces.Add(piece.Id, piece);
            }
        }

        protected virtual void PlaceGoals(IEnumerable<GoalField> goals)
        {
            foreach (var goal in goals)
            {
                if (!(Board[goal] is GoalField))
                    throw new InvalidDataException();

                Board[goal] = goal;
            }
        }

        protected virtual void PlacePlayers(IEnumerable<(PlayerBase player, Location location)> playersWithLocations)
        {
            foreach (var (player, location) in playersWithLocations)
            {
                var playerInfo = new PlayerInfo(player, location);
                Board.Players.Add(playerInfo.Id, playerInfo);
                Board[playerInfo.Location].PlayerId = playerInfo.Id;
            }
        }
    }
}