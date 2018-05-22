﻿using PlayerStateCoordinator.Common;
using PlayerStateCoordinator.Common.Transitions;
using PlayerStateCoordinator.GamePlay.TeamLeader.Transitions;

namespace PlayerStateCoordinator.GamePlay.TeamLeader.States
{
    public class InitLeaderStrategyState : LeaderStrategyState
    {
        public InitLeaderStrategyState(LeaderStrategyInfo leaderStrategyInfo) : base(StateTransitionType.Immediate,
            leaderStrategyInfo)
        {
            Transitions = new Transition[]
            {
                new NearEnemyGoalAreaTransition(leaderStrategyInfo),
                new FarFromEnemyGoalAreaTransition(leaderStrategyInfo)
            };
        }
    }
}