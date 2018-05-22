namespace PlayerStateCoordinator.GamePlay.TeamLeader
{
    public abstract class LeaderStrategyTransition : GameStrategyTransition
    {
        protected LeaderStrategyInfo LeaderStrategyInfo;

        protected LeaderStrategyTransition(LeaderStrategyInfo leaderStrategyInfo) : base(leaderStrategyInfo)
        {
            LeaderStrategyInfo = leaderStrategyInfo;
        }
    }
}