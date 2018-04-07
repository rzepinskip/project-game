using System;
using System.Xml.Serialization;
using Common.Interfaces;

namespace Common.BoardObjects
{

    public abstract class BoardBase : IBoard
    {
        protected BoardBase()
        {
        }

        protected BoardBase(int boardWidth, int taskAreaSize, int goalAreaSize)
        {
            GoalAreaSize = goalAreaSize;
            TaskAreaSize = taskAreaSize;
            Width = boardWidth;
            Content = new Field[boardWidth, Height];
            Players = new SerializableDictionary<int, PlayerInfo>();
            Pieces = new SerializableDictionary<int, Piece>();

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

        [XmlIgnore] public Field[,] Content { get; private set; }

        [XmlArray("Content")]
        [XmlArrayItem(nameof(GoalField), typeof(GoalField))]
        [XmlArrayItem(nameof(TaskField), typeof(TaskField))]
        public Field[] ContentFlattend
        {
            get => Flatten(Content);
            set => Content = Expand(value, Height);
        }

        public object Lock { get; set; } = new object();

        public Field this[Location location]
        {
            get => Content[location.X, location.Y];
            set => Content[location.X, location.Y] = value;
        }

        public int TaskAreaSize { get; }
        public int GoalAreaSize { get; }
        public int Width { get; }
        public int Height => 2 * GoalAreaSize + TaskAreaSize;

        public SerializableDictionary<int, PlayerInfo> Players { get; set; }
        public SerializableDictionary<int, Piece> Pieces { get; }

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
                var field = this[new Location(i, j)] as TaskField;
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
            return location.Y <= TaskAreaSize + GoalAreaSize - 1 && location.Y >= GoalAreaSize;
        }

        public static T[] Flatten<T>(T[,] arr)
        {
            var rows0 = arr.GetLength(0);
            var rows1 = arr.GetLength(1);
            var arrFlattened = new T[rows0 * rows1];
            for (var j = 0; j < rows1; j++)
            for (var i = 0; i < rows0; i++)
                arrFlattened[i + j * rows0] = arr[i, j];

            return arrFlattened;
        }

        public static T[,] Expand<T>(T[] arr, int rows0)
        {
            var length = arr.GetLength(0);
            var rows1 = length / rows0;
            var arrExpanded = new T[rows0, rows1];
            for (var j = 0; j < rows1; j++)
            for (var i = 0; i < rows0; i++)
                arrExpanded[i, j] = arr[i + j * rows0];
            return arrExpanded;
        }

        public void Add(object value)
        {
            throw new NotSupportedException("Add is not supported.");
        }
    }
}