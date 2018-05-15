using System;

namespace Common.Interfaces
{
    public interface IGameMaster
    {
        (BoardData data, bool isGameFinished) EvaluateAction(ActionInfo.ActionInfo actionInfo);

        void SetGameId(int gameId);

        bool IsSlotAvailable();

        (int gameId, Guid playerGuid, PlayerBase playerInfo) AssignPlayerToAvailableSlotWithPrefered(int playerId,
            TeamColor preferedTeam, PlayerType preferedRole);

        void HandlePlayerDisconnection(int playerId);
        void RegisterGame();
        IKnowledgeExchangeManager KnowledgeExchangeManager { get; }
        int? Authorize(Guid playerGuid);
        void SendDataToInitiator(int initiatorId, IMessage message);
        bool PlayerIdExists(int playerId);
    }
}