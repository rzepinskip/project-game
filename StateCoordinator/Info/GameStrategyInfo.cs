using System;
using Common.BoardObjects;

namespace PlayerStateCoordinator.Info
{
    public class GameStrategyInfo : BaseInfo
    {
        public GameStrategyInfo(BoardBase board, int playerId, Guid playerGuid, int gameId)
        {
            Board = board;
            PlayerId = playerId;
            PlayerGuid = playerGuid;
            GameId = gameId;
        }

        public readonly BoardBase Board;
        public readonly int PlayerId;
        public readonly Guid PlayerGuid;
        public readonly int GameId;
    }
}
