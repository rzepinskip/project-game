using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Serialization;
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

        [XmlElement(Order = 10)]
        public List<Location> UncompletedBlueGoalsLocations { get; set; } = new List<Location>();
        [XmlElement(Order = 11)]
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
            else if (UncompletedRedGoalsLocations.Count == 0)
                return TeamColor.Red;
            else
                throw new InvalidOperationException();
        }
    }
}