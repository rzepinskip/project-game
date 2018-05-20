using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Common.Interfaces;

namespace Common.BoardObjects
{
    public abstract class BoardBase : IBoard, IXmlSerializable, IEquatable<BoardBase>
    {
        protected BoardBase()
        {
        }

        protected BoardBase(int boardWidth, int taskAreaSize, int goalAreaSize, GoalFieldType defaultGoalFieldType=GoalFieldType.Unknown)
        {
            GoalAreaSize = goalAreaSize;
            TaskAreaSize = taskAreaSize;
            Width = boardWidth;
            Content = new Field[boardWidth, Height];

            for (var i = 0; i < boardWidth; ++i)
            {
                for (var j = 0; j < goalAreaSize; ++j)
                    Content[i, j] = new GoalField(new Location(i, j), TeamColor.Blue, defaultGoalFieldType);

                for (var j = goalAreaSize; j < taskAreaSize + goalAreaSize; ++j)
                    Content[i, j] = new TaskField(new Location(i, j));

                for (var j = taskAreaSize + goalAreaSize; j < Height; ++j)
                    Content[i, j] = new GoalField(new Location(i, j), TeamColor.Red, defaultGoalFieldType);
            }
        }

        protected Field[,] Content { get; set; }

        public object Lock { get; protected set; } = new object();

        public Field this[Location location]
        {
            get => Content[location.X, location.Y];
            set => Content[location.X, location.Y] = value;
        }

        public int TaskAreaSize { get; private set; }

        public int GoalAreaSize { get; private set; }

        public int Width { get; private set; }

        public int Height => 2 * GoalAreaSize + TaskAreaSize;

        public SerializableDictionary<int, PlayerInfo> Players { get; } =
            new SerializableDictionary<int, PlayerInfo>();


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

        public BoardData ToBoardData(int senderId, int receiverId)
        {
            var taskFields = ToEnumerable().Where(f => f is TaskField taskField && taskField.DistanceToPiece != -1).Select(t => (TaskField)t);
            var goalFields = ToEnumerable().Where(f => f is GoalField goalField).Select(t => (GoalField)t);
        
            var pieces = Pieces.Values.ToArray();

            return BoardData.Create(receiverId, Players[senderId].Location, taskFields.ToArray(), goalFields.ToArray(), pieces);
        }

        public bool Equals(BoardBase other)
        {
            return other != null &&
                   IsContentEqual(other.Content) &&
                   TaskAreaSize == other.TaskAreaSize &&
                   GoalAreaSize == other.GoalAreaSize &&
                   Width == other.Width &&
                   Height == other.Height &&
                   Players.SequenceEqual(other.Players);
        }


        public XmlSchema GetSchema()
        {
            return null;
        }

        public virtual void ReadXml(XmlReader reader)
        {
            reader.MoveToContent();

            TaskAreaSize = int.Parse(reader.GetAttribute("taskAreaSize"));
            GoalAreaSize = int.Parse(reader.GetAttribute("goalAreaSize"));
            Width = int.Parse(reader.GetAttribute("width"));

            reader.ReadStartElement();

            ReadCollection(reader, Players, nameof(Players));

            Content = new Field[Width, Height];
            var types = new[] {typeof(GoalField), typeof(TaskField)};
            var readElements = ReadCollection<Field>(reader, nameof(Content), types);
            foreach (var element in readElements) this[element] = element;
        }

        public IEnumerable<Field> ToEnumerable()
        {
            foreach (var item in Content)
                yield return item;
        }

        public virtual void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("taskAreaSize", TaskAreaSize.ToString());
            writer.WriteAttributeString("goalAreaSize", GoalAreaSize.ToString());
            writer.WriteAttributeString("width", Width.ToString());

            WriteCollection(writer, Players, nameof(Players));

            writer.WriteStartElement(nameof(Content));
            foreach (var field in Content)
            {
                writer.WriteStartElement(field.GetType().Name);
                field.WriteXml(writer);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }

        public void Add(object value)
        {
            throw new NotSupportedException("Add is not supported.");
        }

        protected void ReadCollection<TCollection>(XmlReader reader, TCollection collection, string collectionName)
            where TCollection : IEnumerable, IXmlSerializable
        {
            collection.ReadXml(reader);
        }

        protected void WriteCollection<TCollection>(XmlWriter writer, TCollection collection, string collectionName)
            where TCollection : IEnumerable, IXmlSerializable
        {
            writer.WriteStartElement(collectionName);
            collection.WriteXml(writer);
            writer.WriteEndElement();
        }

        protected List<T> ReadCollection<T>(XmlReader reader, string collectionName, IEnumerable<Type> derivedTypes)
            where T : class
        {
            var serializers = new Dictionary<string, XmlSerializer>();
            foreach (var derivedType in derivedTypes)
                serializers.Add(derivedType.Name, new XmlSerializer(derivedType));

            reader.ReadStartElement(collectionName);

            var readElements = new List<T>();
            while (reader.NodeType != XmlNodeType.EndElement)
            {
                var element = serializers[reader.LocalName].Deserialize(reader) as T;

                readElements.Add(element);
            }

            reader.ReadEndElement();

            return readElements;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as BoardBase);
        }

        private bool IsContentEqual(Field[,] otherContent)
        {
            for (var i = 0; i < Content.GetLength(0); i++)
            for (var j = 0; j < Content.GetLength(1); j++)
                if (Content[i, j] != otherContent[i, j])
                    return false;

            return true;
        }

        public override int GetHashCode()
        {
            var hashCode = -190167123;
            hashCode = hashCode * -1521134295 + EqualityComparer<Field[,]>.Default.GetHashCode(Content);
            hashCode = hashCode * -1521134295 + TaskAreaSize.GetHashCode();
            hashCode = hashCode * -1521134295 + GoalAreaSize.GetHashCode();
            hashCode = hashCode * -1521134295 + Width.GetHashCode();
            hashCode = hashCode * -1521134295 + Height.GetHashCode();
            hashCode = hashCode * -1521134295 +
                       EqualityComparer<Dictionary<int, PlayerInfo>>.Default.GetHashCode(Players);
            return hashCode;
        }

        public static bool operator ==(BoardBase base1, BoardBase base2)
        {
            return EqualityComparer<BoardBase>.Default.Equals(base1, base2);
        }

        public static bool operator !=(BoardBase base1, BoardBase base2)
        {
            return !(base1 == base2);
        }
    }
}