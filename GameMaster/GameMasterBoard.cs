using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Common;
using Common.BoardObjects;
using Common.Interfaces;

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

        public bool Equals(GameMasterBoard other)
        {
            return other != null &&
                   base.Equals(other) &&
                   UncompletedBlueGoalsLocations.SequenceEqual(other.UncompletedBlueGoalsLocations) &&
                   UncompletedRedGoalsLocations.SequenceEqual(other.UncompletedRedGoalsLocations);
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

        public override void ReadXml(XmlReader reader)
        {
            base.ReadXml(reader);

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
    }
}