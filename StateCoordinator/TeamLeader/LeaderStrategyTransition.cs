using System.Collections.Generic;
using Common.Interfaces;
using PlayerStateCoordinator.Common;
using PlayerStateCoordinator.Common.States;
using PlayerStateCoordinator.Common.Transitions;

namespace PlayerStateCoordinator.TeamLeader
{
    public abstract class LeaderStrategyTransition : GameStrategyTransition
    {
        public LeaderStrategyInfo LeaderStrategyInfo;
        public LeaderStrategyTransition(LeaderStrategyInfo leaderStrategyInfo) : base(leaderStrategyInfo)
        {
            LeaderStrategyInfo = leaderStrategyInfo;
        }
    }
}