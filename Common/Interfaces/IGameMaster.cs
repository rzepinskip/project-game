namespace Common.Interfaces
{
    public interface IGameMaster
    {
        (DataFieldSet data, bool isGameFinished) EvaluateAction(ActionInfo.ActionInfo actionInfo);

        void SetGameId(int gameId);
        bool IsLeaderInTeam(TeamColor team);
        bool IsPlaceOnTeam(TeamColor team);
        void AssignPlayerToTeam(int playerId, TeamColor team, PlayerType playerType);



    }
}