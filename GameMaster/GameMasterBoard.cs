using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Common;
using Common.BoardObjects;
using Common.Interfaces;
using Messaging.Serialization;

namespace GameMaster
{
    public class GameMasterBoard : BoardBase, IGameMasterBoard, IEquatable<GameMasterBoard>
    {
        protected GameMasterBoard()
        {
        }

        public GameMasterBoard(int boardWidth, int taskAreaSize, int goalAreaSize) : base(boardWidth, taskAreaSize, goalAreaSize, GoalFieldType.NonGoal)
        {
        }

        public List<Location> UncompletedBlueGoalsLocations { get; set; } = new List<Location>();
        public List<Location> UncompletedRedGoalsLocations { get; set; } = new List<Location>();

        public SerializableDictionary<int, Piece> Pieces { get; } = new SerializableDictionary<int, Piece>();

        public bool Equals(GameMasterBoard other)
        {
            return other != null &&
                   base.Equals(other) &&
                   UncompletedBlueGoalsLocations.SequenceEqual(other.UncompletedBlueGoalsLocations) &&
                   UncompletedRedGoalsLocations.SequenceEqual(other.UncompletedRedGoalsLocations) &&
                   Pieces.SequenceEqual(other.Pieces);
        }

        public void MarkGoalAsCompleted(GoalField goal)
        {
            switch (goal.Team)
            {
                case TeamColor.Blue:
                    UncompletedBlueGoalsLocations.Remove(goal);
                    break;
                case TeamColor.Red:
                    UncompletedRedGoalsLocations.Remove(goal);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public bool IsGameFinished()
        {
            return UncompletedBlueGoalsLocations.Count == 0 || UncompletedRedGoalsLocations.Count == 0;
        }

        public TeamColor CheckWinner()
        {
            if (UncompletedBlueGoalsLocations.Count == 0)
                return TeamColor.Blue;
            if (UncompletedRedGoalsLocations.Count == 0)
                return TeamColor.Red;

            throw new InvalidOperationException();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as GameMasterBoard);
        }

        public override int GetHashCode()
        {
            var hashCode = -961712695;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 +
                       EqualityComparer<List<Location>>.Default.GetHashCode(UncompletedBlueGoalsLocations);
            hashCode = hashCode * -1521134295 +
                       EqualityComparer<List<Location>>.Default.GetHashCode(UncompletedRedGoalsLocations);
            hashCode = hashCode * -1521134295 + EqualityComparer<Dictionary<int, Piece>>.Default.GetHashCode(Pieces);
            return hashCode;
        }

        public static bool operator ==(GameMasterBoard board1, GameMasterBoard board2)
        {
            return EqualityComparer<GameMasterBoard>.Default.Equals(board1, board2);
        }

        public static bool operator !=(GameMasterBoard board1, GameMasterBoard board2)
        {
            return !(board1 == board2);
        }

        public override void WriteXml(XmlWriter writer)
        {
            base.WriteXml(writer);
            WriteCollection(writer, Pieces, nameof(Pieces));
            WriteUncompletedGoalsLocations(writer, UncompletedBlueGoalsLocations,
                nameof(UncompletedBlueGoalsLocations));
            WriteUncompletedGoalsLocations(writer, UncompletedRedGoalsLocations,
                nameof(UncompletedRedGoalsLocations));
        }

        private void WriteUncompletedGoalsLocations(XmlWriter writer, List<Location> locations, string name)
        {
            writer.WriteStartElement(name);
            foreach (var location in locations)
            {
                writer.WriteStartElement(nameof(Location));
                location.WriteXml(writer);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }

        public override void ReadXml(XmlReader reader)
        {
            base.ReadXml(reader);
            ReadCollection(reader, Pieces, nameof(Pieces));

            if (reader.NodeType != XmlNodeType.EndElement)
            {
                var blueGoals = ReadCollection<Location>(reader, nameof(UncompletedBlueGoalsLocations), new[] { typeof(Location) });
                UncompletedBlueGoalsLocations.AddRange(blueGoals);

                var readGoals = ReadCollection<Location>(reader, nameof(UncompletedRedGoalsLocations), new[] { typeof(Location) });
                UncompletedRedGoalsLocations.AddRange(readGoals);
            }
            else
            {
                foreach (var field in Content)
                {
                    if (!(field is GoalField goalField) || goalField.Type != GoalFieldType.Goal) continue;

                    switch (goalField.Team)
                    {
                        case TeamColor.Blue:
                            UncompletedBlueGoalsLocations.Add(goalField);
                            break;
                        case TeamColor.Red:
                            UncompletedRedGoalsLocations.Add(goalField);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            reader.ReadEndElement();
        }

        public override BoardData ToBoardData(int senderId, int receiverId)
        {
            var taskFields = ToEnumerable().Where(f => f is TaskField taskField).Select(t => (TaskField)t);
            var goalFields = ToEnumerable().Where(f => f is GoalField goalField).Select(t => (GoalField)t);
            var pieces = Pieces.Values;
            var playerLocation = Players[receiverId].Location;
            return BoardData.Create(receiverId, playerLocation, taskFields.ToArray(), goalFields.ToArray(), pieces.ToArray());
        }
    }
}