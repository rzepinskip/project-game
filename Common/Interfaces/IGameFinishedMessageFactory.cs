using System.Collections.Generic;
using Common.BoardObjects;

namespace Common.Interfaces
{
    public interface IGameMasterMessageFactory
    {
        IMessage CreateGameResultsMessage(BoardData boardData);

        IMessage CreateGameMessage(int playerId, IEnumerable<PlayerBase> playersInGame, Location playerLocation, BoardInfo boardInfo);

        IMessage CreateGameStartedMessage(int gameId);

        IMessage CreateRegisterGameMessage(GameInfo gameInfo);
    }
}
