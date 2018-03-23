using System;
using System.Collections.Generic;
using System.Linq;
using GameMaster.Configuration;
using Common;
using Common.BoardObjects;
using GameMaster.Delays;
using Xunit;

namespace GameMaster.Tests
{
    public class ConfigurationLoaderTests
    {
        public ConfigurationLoaderTests()
        {
            _confTest = new GameConfiguration
            {
                KeepAliveInterval = 500,
                RetryRegisterGameInterval = 60000,
                ActionCosts = new ActionCosts
                {
                    MoveDelay = 10,
                    DiscoverDelay = 45,
                    TestDelay = 50,
                    PickUpDelay = 10,
                    PlacingDelay = 10,
                    KnowledgeExchangeDelay = 500
                },
                GameDefinition = new GameDefinition
                {
                    ShamProbability = 0.33,
                    PlacingNewPiecesFrequency = 200,
                    InitialNumberOfPieces = 10,
                    BoardWidth = 10,
                    TaskAreaLength = 10,
                    GoalAreaLength = 3,
                    NumberOfPlayersPerTeam = 8,
                    GameName = "Endgame",
                    Goals = new List<GoalField>
                    {
                        new GoalField(new Location(4,15),TeamColor.Red,GoalFieldType.Goal,null, DateTime.MinValue),
                        new GoalField(new Location(6,1),TeamColor.Blue, GoalFieldType.Goal,null, DateTime.MinValue )
                    }
                }
            };

            var cl = new ConfigurationLoader();
            _conf = cl.LoadConfigurationFromFile("Resources/ConfigurationLoaderTestsData.xml");
        }

        private readonly GameConfiguration _conf;
        private readonly GameConfiguration _confTest;

        [Fact]
        public void BoardWidthLoaded()
        {
            Assert.Equal(_confTest.GameDefinition.BoardWidth, _conf.GameDefinition.BoardWidth);
        }

        [Fact]
        public void DiscoverDelayLoaded()
        {
            Assert.Equal(_confTest.ActionCosts.DiscoverDelay, _conf.ActionCosts.DiscoverDelay);
        }

        [Fact]
        public void GameNameLoaded()
        {
            Assert.Equal(_confTest.GameDefinition.GameName, _conf.GameDefinition.GameName);
        }

        [Fact]
        public void GoalAreaLengthLoaded()
        {
            Assert.Equal(_confTest.GameDefinition.GoalAreaLength, _conf.GameDefinition.GoalAreaLength);
        }

        [Fact]
        public void GoalsLoaded()
        {
            Assert.True(_confTest.GameDefinition.Goals == null && _conf.GameDefinition.Goals == null ||
                        _confTest.GameDefinition.Goals != null && _conf.GameDefinition.Goals != null);
            Assert.Equal(_confTest.GameDefinition.Goals.Count, _conf.GameDefinition.Goals.Count);

            var qwe = _confTest.GameDefinition.Goals.Zip(_conf.GameDefinition.Goals, (a, b) => a == b).ToList();
            var result = _confTest.GameDefinition.Goals.Zip(_conf.GameDefinition.Goals, (a, b) => a == b)
                .Aggregate((a, b) => a && b);
            Assert.True(result);
        }

        [Fact]
        public void InitialNumberOfPiecesLoaded()
        {
            Assert.Equal(_confTest.GameDefinition.InitialNumberOfPieces, _conf.GameDefinition.InitialNumberOfPieces);
        }

        [Fact]
        public void KeepAliveIntervalLoaded()
        {
            Assert.Equal(_confTest.KeepAliveInterval, _conf.KeepAliveInterval);
        }

        [Fact]
        public void KnowledgeExchangeDelayLoaded()
        {
            Assert.Equal(_confTest.ActionCosts.KnowledgeExchangeDelay, _conf.ActionCosts.KnowledgeExchangeDelay);
        }

        [Fact]
        public void MoveDelayLoaded()
        {
            Assert.Equal(_confTest.ActionCosts.MoveDelay, _conf.ActionCosts.MoveDelay);
        }

        [Fact]
        public void NumberOfPlayersPerTeamLoaded()
        {
            Assert.Equal(_confTest.GameDefinition.NumberOfPlayersPerTeam, _conf.GameDefinition.NumberOfPlayersPerTeam);
        }

        [Fact]
        public void PickUpDelayLoaded()
        {
            Assert.Equal(_confTest.ActionCosts.PickUpDelay, _conf.ActionCosts.PickUpDelay);
        }

        [Fact]
        public void PlacingDelayLoaded()
        {
            Assert.Equal(_confTest.ActionCosts.PlacingDelay, _conf.ActionCosts.PlacingDelay);
        }

        [Fact]
        public void PlacingNewPiecesFrequencyLoaded()
        {
            Assert.Equal(_confTest.GameDefinition.PlacingNewPiecesFrequency,
                _conf.GameDefinition.PlacingNewPiecesFrequency);
        }

        [Fact]
        public void RetryRegisterGameIntervalLoaded()
        {
            Assert.Equal(_confTest.RetryRegisterGameInterval, _conf.RetryRegisterGameInterval);
        }

        [Fact]
        public void ShamProbabilityLoaded()
        {
            Assert.Equal(_confTest.GameDefinition.ShamProbability, _conf.GameDefinition.ShamProbability);
        }

        [Fact]
        public void TaskAreaLengthLoaded()
        {
            Assert.Equal(_confTest.GameDefinition.TaskAreaLength, _conf.GameDefinition.TaskAreaLength);
        }


        [Fact]
        public void TestDelayLoaded()
        {
            Assert.Equal(_confTest.ActionCosts.TestDelay, _conf.ActionCosts.TestDelay);
        }
    }
}