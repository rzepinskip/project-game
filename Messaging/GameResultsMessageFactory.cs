using System.Collections.Generic;
using Common;
using Common.BoardObjects;
using Common.Interfaces;
using Messaging.InitializationMessages;

namespace Messaging
{
    public class GameMasterMessageFactory : IGameMasterMessageFactory
    {
        public IMessage CreateGameResultsMessage(BoardData boardData)
        {
            return DataMessage.FromBoardData(boardData, true);
        }

        public IMessage CreateGameMessage(int playerId, IEnumerable<PlayerBase> playersInGame, Location playerLocation,
            BoardInfo boardInfo)
        {
            return new GameMessage(playerId, playersInGame, playerLocation, boardInfo);
        }

        public IMessage CreateGameStartedMessage(int gameId)
        {
            return new GameStartedMessage(gameId);
        }

        public IMessage CreateRegisterGameMessage(GameInfo gameInfo)
        {
            return new RegisterGameMessage(gameInfo);
        }
    }
}
