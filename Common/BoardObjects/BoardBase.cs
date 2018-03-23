using System.Collections;
using System.Collections.Generic;
using Common.Interfaces;

namespace Common.BoardObjects
{
    public abstract class BoardBase : IBoard
    {
        protected BoardBase(int boardWidth, int taskAreaSize, int goalAreaSize)
        {
            GoalAreaSize = goalAreaSize;
            TaskAreaSize = taskAreaSize;
            Width = boardWidth;
            Content = new Field[boardWidth, Height];
            Players = new Dictionary<int, PlayerInfo>();
            Pieces = new Dictionary<int, Piece>();

            for (var i = 0; i < boardWidth; ++i)
            {
                for (var j = 0; j < goalAreaSize; ++j)
                    Content[i, j] = new GoalField(new Location(i, j), TeamColor.Blue);

                for (var j = goalAreaSize; j < taskAreaSize + goalAreaSize; ++j)
                    Content[i, j] = new TaskField(new Location(i, j));

                for (var j = taskAreaSize + goalAreaSize; j < Height; ++j)
                    Content[i, j] = new GoalField(new Location(i, j), TeamColor.Red);
            }
        }

        protected Field[,] Content { get; }

        public Field this[Location location]
        {
            get => Content[location.X, location.Y];
            set => Content[location.X, location.Y] = value;
        }

        public int TaskAreaSize { get; }
        public int GoalAreaSize { get; }
        public int Width { get; }
        public int Height => 2 * GoalAreaSize + TaskAreaSize;

        public Dictionary<int, PlayerInfo> Players { get; }
        public Dictionary<int, Piece> Pieces { get; }
        public object Lock { get; set; } = new object();

        public IEnumerator GetEnumerator()
        {
            return Content.GetEnumerator();
        }

        public int? GetPieceIdAt(Location location)
        {
            int? pieceId = null;

            if (IsLocationInTaskArea(location))
                pieceId = (this[location] as TaskField).PieceId;

            return pieceId;
        }


        public int DistanceToPieceFrom(Location location)
        {
            var min = int.MaxValue;
            for (var i = 0; i < Width; ++i)
            for (var j = GoalAreaSize; j < TaskAreaSize + GoalAreaSize; ++j)
            {
                var field = Content[i, j] as TaskField;
                if (field.PieceId != null)
                {
                    var distance = field.ManhattanDistanceTo(location);
                    if (distance < min)
                        min = distance;
                }
            }

            if (min == int.MaxValue)
                min = -1;

            return min;
        }

        public bool IsLocationInTaskArea(Location location)
        {
            if (location.Y <= TaskAreaSize + GoalAreaSize - 1 && location.Y >= GoalAreaSize)
                return true;

            return false;
        }
    }
}