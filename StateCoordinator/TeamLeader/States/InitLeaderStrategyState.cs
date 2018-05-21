using PlayerStateCoordinator.Common;
using PlayerStateCoordinator.Common.Transitions;
using PlayerStateCoordinator.TeamLeader.Transitions;

namespace PlayerStateCoordinator.TeamLeader.States
{
    public class InitLeaderStrategyState : LeaderStrategyState
    {
        public InitLeaderStrategyState(GameStrategyInfo gameStrategyInfo) : base(StateTransitionType.Immediate,
            gameStrategyInfo)
        {
            Transitions = new Transition[]
            {
                new NearEnemyGoalAreaTransition(gameStrategyInfo),
                new FarFromEnemyGoalAreaTransition(GameStrategyInfo)
            };
        }
    }
}