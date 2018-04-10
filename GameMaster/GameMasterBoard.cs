using System;
using System.Collections.Generic;
using System.Xml;
using Common;
using Common.BoardObjects;
using Common.Interfaces;

namespace GameMaster
{
    public class GameMasterBoard : BoardBase, IGameMasterBoard
    {
        protected GameMasterBoard()
        {

        }

        public GameMasterBoard(int boardWidth, int taskAreaSize, int goalAreaSize) : base(boardWidth, taskAreaSize, goalAreaSize)
        {
        }

        public List<Location> UncompletedBlueGoalsLocations { get; set; } = new List<Location>();
        public List<Location> UncompletedRedGoalsLocations { get; set; } = new List<Location>();

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