using System;
using System.Collections.Generic;

namespace Common
{
    public abstract class BoardBase
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
                    Content[i, j] = new GoalField(CommonResources.GoalFieldType.Unknown,
                        CommonResources.TeamColour.Blue, null, DateTime.Now, i, j);
                for (var j = goalAreaSize; j < taskAreaSize + goalAreaSize; ++j)
                    Content[i, j] = new TaskField(-1, null, null, DateTime.Now, i, j);
                for (var j = taskAreaSize + goalAreaSize; j < Height; ++j)
                    Content[i, j] = new GoalField(CommonResources.GoalFieldType.Unknown, CommonResources.TeamColour.Red,
                        null, DateTime.Now, i, j);
            }
        }

        protected Field[,] Content { get; }
        public Field this[Location location]
        {
            get => Content[location.X, location.Y];
            protected set => Content[location.X, location.Y] = value;
        }

        public int TaskAreaSize { get; }
        public int GoalAreaSize { get; }
        public int Width { get; }
        public int Height => 2 * GoalAreaSize + TaskAreaSize;

        public Dictionary<int, PlayerInfo> Players { get; }
        public Dictionary<int, Piece> Pieces { get; }
    }
}