using GameMaster.Configuration;
using Shared.Board;
using System.Xml;
using Xunit;
using Shared;
using System.Xml.Serialization;
using System.IO;

namespace GameMaster.Tests
{
    public class ConfigurationLoaderTests
    {
        GameConfiguration conf;

        public ConfigurationLoaderTests()
        {
            ConfigurationLoader cl = new ConfigurationLoader();
            conf = cl.LoadConfigurationFromText(FILECONTENT);
        }

        [Fact]
        public void GameMasterAttributesLoaded()
        {
            Assert.Equal(500, conf.KeepAliveInterval);
            Assert.Equal(60000, conf.RetryRegisterGameInterval);
        }

        [Fact]
        public void GoalsLoaded()
        {
            Assert.NotNull(conf.GameDefinition.Goals);
            Assert.Equal(2, conf.GameDefinition.Goals.Count);

            var redGoal = new GoalField { Team = CommonResources.Team.Red, X = 4, Y = 15, Type = CommonResources.GoalFieldType.Goal };
            var blueGoal = new GoalField { Team = CommonResources.Team.Blue, X = 6, Y = 1, Type = CommonResources.GoalFieldType.Goal };


            var first = conf.GameDefinition.Goals[0];
            var second = conf.GameDefinition.Goals[1];

            Assert.Equal(redGoal.Team, first.Team);
            Assert.Equal(redGoal.Type, first.Type);
            Assert.Equal(redGoal.X, first.X);
            Assert.Equal(redGoal.Y, first.Y);

            Assert.Equal(blueGoal.Team, second.Team);
            Assert.Equal(blueGoal.Type, second.Type);
            Assert.Equal(blueGoal.X, second.X);
            Assert.Equal(blueGoal.Y, second.Y);
        }

        [Fact]
        public void GameDefinitionLoaded()
        {
            Assert.Equal(0.33, conf.GameDefinition.ShamProbability);
            Assert.Equal(200, conf.GameDefinition.PlacingNewPiecesFrequency);
            Assert.Equal(10, conf.GameDefinition.InitialNumberOfPieces);
            Assert.Equal(10, conf.GameDefinition.BoardWidth);
            Assert.Equal(10, conf.GameDefinition.TaskAreaLength);
            Assert.Equal(3, conf.GameDefinition.GoalAreaLength);
            Assert.Equal(8, conf.GameDefinition.NumberOfPlayersPerTeam);
            Assert.Equal("Endgame", conf.GameDefinition.GameName);
        }

        [Fact]
        public void ActionCostsLoaded()
        {
            Assert.Equal(10, conf.ActionCosts.MoveDelay);
            Assert.Equal(45, conf.ActionCosts.DiscoverDelay);
            Assert.Equal(50, conf.ActionCosts.TestDelay);
            Assert.Equal(10, conf.ActionCosts.PickUpDelay);
            Assert.Equal(10, conf.ActionCosts.PlacingDelay);
            Assert.Equal(500, conf.ActionCosts.KnowledgeExchangeDelay);
        }



        const string FILECONTENT = @"
<GameMasterSettings xmlns = ""https://se2.mini.pw.edu.pl/17-pl-19/17-pl-19/""
                    KeepAliveInterval=""500""
                    RetryRegisterGameInterval=""60000"">
  <GameDefinition>
    <Goals team=""red"" x=""4"" y =""15"" type=""goal"" />
    <Goals team=""blue"" x=""6"" y =""1"" type=""goal"" />
    <ShamProbability>0.33</ShamProbability>
    <PlacingNewPiecesFrequency>200</PlacingNewPiecesFrequency>
    <InitialNumberOfPieces>10</InitialNumberOfPieces>
    <BoardWidth>10</BoardWidth>
    <TaskAreaLength>10</TaskAreaLength>
    <GoalAreaLength>3</GoalAreaLength>
    <NumberOfPlayersPerTeam>8</NumberOfPlayersPerTeam>
    <GameName>Endgame</GameName>
  </GameDefinition>
  <ActionCosts>
    <MoveDelay>10</MoveDelay>
    <DiscoverDelay>45</DiscoverDelay>
    <TestDelay>50</TestDelay>
    <PickUpDelay>10</PickUpDelay>
    <PlacingDelay>10</PlacingDelay>
    <KnowledgeExchangeDelay>500</KnowledgeExchangeDelay>
  </ActionCosts>
</GameMasterSettings>";
    }
}
