using PlayerStateCoordinator.Common.Transitions;

namespace PlayerStateCoordinator.GamePlay.TeamLeader
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