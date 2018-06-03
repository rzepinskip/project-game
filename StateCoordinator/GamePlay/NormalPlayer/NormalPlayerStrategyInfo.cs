using System;
using System.Collections.Generic;
using Common;
using Common.BoardObjects;

namespace PlayerStateCoordinator.GamePlay.NormalPlayer
{
    public class NormalPlayerStrategyInfo : GamePlayStrategyInfo
    {
        private const int KnowledgeExchangeInterval = 2;
        public readonly List<GoalField> UndiscoveredGoalFields;

        private int _skippedKnowledgeExchangeCounter;

        public NormalPlayerStrategyInfo(BoardBase board, int playerId, Guid playerGuid, int gameId, TeamColor team,
            List<GoalField> undiscoveredGoalFields) : base(board, playerId, playerGuid, gameId, team)
        {
            UndiscoveredGoalFields = undiscoveredGoalFields;
            _skippedKnowledgeExchangeCounter = 0;
        }


        public virtual bool ShouldInitiateKnowledgeExchange()
        {
            var result = false;
            _skippedKnowledgeExchangeCounter++;
            if (_skippedKnowledgeExchangeCounter >= KnowledgeExchangeInterval)
            {
                _skippedKnowledgeExchangeCounter = 0;
                result = true;
            }

            return result;
        }
    }
}