using System;

namespace Common.Interfaces
{
    public interface IGameMaster
    {
        (DataFieldSet data, bool isGameFinished) EvaluateAction(ActionInfo.ActionInfo actionInfo);

        void SetGameId(int gameId);

        bool IsSlotAvailable();

        (int gameId, Guid playerGuid, PlayerBase playerInfo) AssignPlayerToAvailableSlotWithPrefered(int playerId,
            TeamColor preferedTeam, PlayerType preferedRole);
    }
}