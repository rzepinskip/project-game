using GameMaster.Configuration;
using Shared.Board;
using System.Xml;
using Xunit;
using Shared;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace GameMaster.Tests
{
    public class ConfigurationLoaderTests
    {
        GameConfiguration conf, confTest;

        public ConfigurationLoaderTests()
        {
            confTest = new GameConfiguration
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
                        new GoalField { Team = CommonResources.Team.Red, X = 4, Y = 15, Type = CommonResources.GoalFieldType.Goal },
                        new GoalField { Team = CommonResources.Team.Blue, X = 6, Y = 1, Type = CommonResources.GoalFieldType.Goal }
                    }
                }
        };                  

            ConfigurationLoader cl = new ConfigurationLoader();
            conf = cl.LoadConfigurationFromFile("Resources/ConfigurationLoaderTestsData.xml");
        }

        [Fact]
        public void KeepAliveIntervalLoaded()
        {
            Assert.Equal(confTest.KeepAliveInterval, conf.KeepAliveInterval);
        }

        [Fact]
        public void GoalsLoaded()
        {
            Assert.True((confTest.GameDefinition.Goals == null && conf.GameDefinition.Goals == null) || (confTest.GameDefinition.Goals != null && conf.GameDefinition.Goals != null));
            Assert.Equal(confTest.GameDefinition.Goals.Count, conf.GameDefinition.Goals.Count);

            var result = confTest.GameDefinition.Goals.Zip(conf.GameDefinition.Goals, (a, b) => a == b).Aggregate((a, b) => a && b);
            Assert.True(result);
        }

        [Fact]
        public void RetryRegisterGameIntervalLoaded()
        {
            Assert.Equal(confTest.RetryRegisterGameInterval, conf.RetryRegisterGameInterval);
        }

        [Fact]
        public void ShamProbabilityLoaded()
        {
            Assert.Equal(confTest.GameDefinition.ShamProbability, conf.GameDefinition.ShamProbability);
        }

        [Fact]
        public void PlacingNewPiecesFrequencyLoaded()
        {
            Assert.Equal(confTest.GameDefinition.PlacingNewPiecesFrequency, conf.GameDefinition.PlacingNewPiecesFrequency);
        }

        [Fact]
        public void InitialNumberOfPiecesLoaded()
        {
            Assert.Equal(confTest.GameDefinition.InitialNumberOfPieces, conf.GameDefinition.InitialNumberOfPieces);
        }

        [Fact]
        public void BoardWidthLoaded()
        {
            Assert.Equal(confTest.GameDefinition.BoardWidth, conf.GameDefinition.BoardWidth);
        }

        [Fact]
        public void TaskAreaLengthLoaded()
        {
            Assert.Equal(confTest.GameDefinition.TaskAreaLength, conf.GameDefinition.TaskAreaLength);
        }

        [Fact]
        public void GoalAreaLengthLoaded()
        {
            Assert.Equal(confTest.GameDefinition.GoalAreaLength, conf.GameDefinition.GoalAreaLength);
        }

        [Fact]
        public void NumberOfPlayersPerTeamLoaded()
        {
            Assert.Equal(confTest.GameDefinition.NumberOfPlayersPerTeam, conf.GameDefinition.NumberOfPlayersPerTeam);
        }

        [Fact]
        public void GameNameLoaded()
        {
            Assert.Equal(confTest.GameDefinition.GameName, conf.GameDefinition.GameName);
        }

        [Fact]
        public void MoveDelayLoaded()
        {
            Assert.Equal(confTest.ActionCosts.MoveDelay, conf.ActionCosts.MoveDelay);
        }

        [Fact]
        public void DiscoverDelayLoaded()
        {
            Assert.Equal(confTest.ActionCosts.DiscoverDelay, conf.ActionCosts.DiscoverDelay);
        }


        [Fact]
        public void TestDelayLoaded()
        {
            Assert.Equal(confTest.ActionCosts.TestDelay, conf.ActionCosts.TestDelay);
        }

        [Fact]
        public void PickUpDelayLoaded()
        {
            Assert.Equal(confTest.ActionCosts.PickUpDelay, conf.ActionCosts.PickUpDelay);
        }

        [Fact]
        public void PlacingDelayLoaded()
        {
            Assert.Equal(confTest.ActionCosts.PlacingDelay, conf.ActionCosts.PlacingDelay);
        }

        [Fact]
        public void KnowledgeExchangeDelayLoaded()
        {
            Assert.Equal(confTest.ActionCosts.KnowledgeExchangeDelay, conf.ActionCosts.KnowledgeExchangeDelay);
        }
    }
}
