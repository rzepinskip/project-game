using PlayerStateCoordinator.Common;
using PlayerStateCoordinator.Common.Transitions;
using PlayerStateCoordinator.NormalPlayer.Transitions;
using PlayerStateCoordinator.TeamLeader.Transitions;

namespace PlayerStateCoordinator.TeamLeader.States
{
    public class MovingTowardsEnemyGoalAreaStrategyState : LeaderStrategyState
    {
        public MovingTowardsEnemyGoalAreaStrategyState(LeaderStrategyInfo leaderStrategyInfo) : base(
            StateTransitionType.Triggered, leaderStrategyInfo)
        {
            Transitions = new Transition[]
            {
                new IsLeaderBlockedTransition(leaderStrategyInfo, this),
                new NearEnemyGoalAreaTransition(leaderStrategyInfo),
                new FarFromEnemyGoalAreaTransition(leaderStrategyInfo)
            };
        }
    }
}