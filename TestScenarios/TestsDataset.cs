﻿using System.Collections;
using System.Collections.Generic;
using TestScenarios.DiscoverScenarios.DiscoverBoardCorner;
using TestScenarios.DiscoverScenarios.DiscoverGoalAreaCorner;
using TestScenarios.DiscoverScenarios.DiscoverGoalAreaEdge;
using TestScenarios.DiscoverScenarios.DiscoverPiece;
using TestScenarios.DiscoverScenarios.DiscoverPieceDisappearance;
using TestScenarios.DiscoverScenarios.DiscoverPlayer;
using TestScenarios.DiscoverScenarios.DiscoverPlayerDisappearance;
using TestScenarios.DiscoverScenarios.DiscoverRegular;
using TestScenarios.DiscoverScenarios.DiscoverTaskAreaBoardEdge;
using TestScenarios.DiscoverScenarios.DiscoverTaskAreaCorner;
using TestScenarios.DiscoverScenarios.DiscoverTaskAreaEdge;
using TestScenarios.MoveScenarios.MoveToGoalField;
using TestScenarios.MoveScenarios.MoveToTasFieldWithoutPiece;
using TestScenarios.MoveScenarios.MoveToTaskFieldOccupiedByPlayerWhoCarryPiece;
using TestScenarios.MoveScenarios.MoveToTaskFieldOccupiedByPlayerWhoDoesntCarryPiece;
using TestScenarios.MoveScenarios.MoveToTaskFieldWithPiece;
using TestScenarios.MoveScenarios.MoveToTaskFieldWithPieceOccupiedByPlayerWhoCarryPiece;
using TestScenarios.MoveScenarios.MoveToTaskFieldWithPieceOccupiedByPlayerWhoDoesntCarryPiece;

namespace TestScenarios
{
    public class TestsDataset : IEnumerable<object[]>
    {
        private readonly List<object[]> _testScenarios = new List<object[]>();

        public TestsDataset()
        {
            _testScenarios.AddRange(GetMoveTests());
            _testScenarios.AddRange(GetDiscoverTests());
        }

        public IEnumerator<object[]> GetEnumerator()
        {
            return _testScenarios.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private IEnumerable<object[]> GetMoveTests()
        {
            return new List<object[]>
            {
                new object[] {new MoveToGoalField()},
                new object[] {new MoveToTaskFieldWithoutPiece()},
                new object[] {new MoveToTaskFieldWithPiece()},
                new object[] { new MoveToTaskFieldOccupiedByPlayerWhoCarryPiece()},
                new object[] {new MoveToTaskFieldOccupiedByPlayerWhoDoesntCarryPiece()},
                new object[] { new MoveToTaskFieldWithPieceOccupiedByPlayerWhoCarryPiece()},
                new object[] {new MoveToTaskFieldWithPieceOccupiedByPlayerWhoDoesntCarryPiece()}
            };
        }

        private IEnumerable<object[]> GetDiscoverTests()
        {
            return new List<object[]>
            {
                new object[] {new DiscoverRegular()},
                new object[] {new DiscoverTaskAreaEdge()},
                new object[] {new DiscoverTaskAreaBoardEdge()},
                new object[] {new DiscoverTaskAreaCorner()},
                new object[] {new DiscoverGoalAreaCorner()},
                new object[] {new DiscoverGoalAreaEdge()},
                new object[] {new DiscoverBoardCorner()},
                new object[] {new DiscoverPiece()},
                new object[] {new DiscoverPiecePickUp()},
                new object[] {new DiscoverPlayer()},
                new object[] {new DiscoverPlayerDisappearance()},
            };
        }
    }
}