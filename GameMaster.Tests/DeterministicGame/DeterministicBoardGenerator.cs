using GameMaster.Tests.BoardConfigurationGenerator;

namespace GameMaster.Tests.DeterministicGame
{
    public class DeterministicBoardGenerator : BoardGenerator
    {
        public GameMasterBoard InitializeBoard(DeterministicGameDefinition config)
        {
            //Board = new GameMasterBoard(config.BoardWidth, config.TaskAreaLength,
            //    config.GoalAreaLength);

            ////var pieces = GeneratePieces(config.PiecesLocations.Count, 0);
            ////var piecesLocations = config.PiecesLocations;
            ////PlacePieces(pieces, piecesLocations);

            //PlaceGoals(config.Goals);

            //var players = GeneratePlayers(config.BluePlayersLocations.Count);
            //var playersLocations = new Dictionary<TeamColor, List<Location>>
            //{
            //    {TeamColor.Blue, config.BluePlayersLocations},
            //    {TeamColor.Red, config.RedPlayersLocations}
            //};
            //PlacePlayers(players, playersLocations);

            return Board;
        }
    }
}
