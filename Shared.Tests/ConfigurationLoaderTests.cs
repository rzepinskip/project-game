using System.Xml;
using Xunit;
namespace Shared.Tests
{
    public class ConfigurationLoaderTests
    {
        GameConfiguration conf;
        
        public ConfigurationLoaderTests()
        {
            var doc = new XmlDocument();
            var cl = new ConfigurationLoader();
            doc.LoadXml(FILECONTENT);

            //Act
            conf = cl.LoadConfiguration(doc);
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
            Assert.NotNull(conf.Goals);
            Assert.Equal(2,conf.Goals.Count);

            var redGoal = new MockGoal { Team = "red", X = 4, Y = 15, Type = "goal" };
            var blueGoal = new MockGoal { Team = "blue", X = 6, Y = 1, Type = "goal" };

            Assert.Equal(redGoal, conf.Goals[0]);
            Assert.Equal(blueGoal, conf.Goals[1]);
        }

        [Fact]
        public void GameDefinitionLoaded()
        {
            Assert.Equal(0.33, conf.ShamProbability);
            Assert.Equal(200, conf.PlacingNewPiecesFrequency);
            Assert.Equal(10, conf.InitialNumberOfPieces);
            Assert.Equal(10, conf.BoardWidth);
            Assert.Equal(10, conf.TaskAreaLength);
            Assert.Equal(3, conf.GoalAreaLength);
            Assert.Equal(8, conf.NumberOfPlayersPerTeam);
            Assert.Equal("Endgame", conf.GameName);
        }



        const string FILECONTENT = @"
<GameMasterSettings xmlns = ""https://se2.mini.pw.edu.pl/17-pl-19/17-pl-19/""
                    KeepAliveInterval=""500""
                    RetryRegisterGameInterval=""60000"">
  <GameDefinition>
    <Goals team = ""red"" x=""4"" y =""15"" type=""goal"" />
    <Goals team = ""blue"" x=""6"" y=""1"" type=""goal"" />
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
