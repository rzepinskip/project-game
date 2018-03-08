using GameMaster.Configuration;
using Shared.BoardObjects;
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
            Assert.NotNull(conf.GD.Goals);
            Assert.Equal(2, conf.GD.Goals.Count);

            var redGoal = new GoalField { Team = CommonResources.Team.Red, X = 4, Y = 15, Type = CommonResources.GoalFieldType.Goal };
            var blueGoal = new GoalField { Team = CommonResources.Team.Blue, X = 6, Y = 1, Type = CommonResources.GoalFieldType.Goal };

            Assert.Equal(redGoal.Team, conf.GD.Goals[0].Team);
            Assert.Equal(redGoal.Type, conf.GD.Goals[0].Type);
            Assert.Equal(redGoal.X, conf.GD.Goals[0].X);
            Assert.Equal(redGoal.Y, conf.GD.Goals[0].Y);

            Assert.Equal(blueGoal.Team, conf.GD.Goals[1].Team);
            Assert.Equal(blueGoal.Type, conf.GD.Goals[1].Type);
            Assert.Equal(blueGoal.X, conf.GD.Goals[1].X);
            Assert.Equal(blueGoal.Y, conf.GD.Goals[1].Y);
        }

        [Fact]
        public void GameDefinitionLoaded()
        {
            Assert.Equal(0.33, conf.GD.ShamProbability);
            Assert.Equal(200, conf.GD.PlacingNewPiecesFrequency);
            Assert.Equal(10, conf.GD.InitialNumberOfPieces);
            Assert.Equal(10, conf.GD.BoardWidth);
            Assert.Equal(10, conf.GD.TaskAreaLength);
            Assert.Equal(3, conf.GD.GoalAreaLength);
            Assert.Equal(8, conf.GD.NumberOfPlayersPerTeam);
            Assert.Equal("Endgame", conf.GD.GameName);
        }

        [Fact]
        public void ActionCostsLoaded()
        {
            Assert.Equal(10, conf.AC.MoveDelay);
            Assert.Equal(45, conf.AC.DiscoverDelay);
            Assert.Equal(50, conf.AC.TestDelay);
            Assert.Equal(10, conf.AC.PickUpDelay);
            Assert.Equal(10, conf.AC.PlacingDelay);
            Assert.Equal(500, conf.AC.KnowledgeExchangeDelay);
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
