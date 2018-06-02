using System;
using System.Collections.Generic;
using Common;
using Common.BoardObjects;
using PlayerStateCoordinator.GamePlay.NormalPlayer;

namespace PlayerStateCoordinator.GamePlay.BasicPlayer
{
    public class BasicStrategyInfo : NormalPlayerStrategyInfo
    {
        public BasicStrategyInfo(BoardBase board, int playerId, Guid playerGuid, int gameId, TeamColor team,
            List<GoalField> undiscoveredGoalFields) : base(board, playerId, playerGuid, gameId, team, undiscoveredGoalFields)
        {
        }

        public override bool ShouldInitiateKnowledgeExchange()
        {
            return false;
        }
    }
}