﻿using System;
using Common;
using Common.BoardObjects;
using PlayerStateCoordinator.Common;

namespace PlayerStateCoordinator.TeamLeader
{
    public class LeaderStrategyInfo : GamePlayStrategyInfo
    {

        protected LeaderStrategyInfo(BoardBase board, int playerId, Guid playerGuid, int gameId, TeamColor team) : base(board, playerId, playerGuid, gameId, team)
        {
        }
    }
}