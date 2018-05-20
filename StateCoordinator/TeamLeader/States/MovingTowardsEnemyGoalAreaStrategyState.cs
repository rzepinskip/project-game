using PlayerStateCoordinator.Common;
using PlayerStateCoordinator.Common.Transitions;
using PlayerStateCoordinator.Info;
using PlayerStateCoordinator.NormalPlayer.Transitions;
using PlayerStateCoordinator.TeamLeader.Transitions;

namespace PlayerStateCoordinator.TeamLeader.States
{
    public class MovingTowardsEnemyGoalAreaStrategyState : LeaderStrategyState
    {
        public MovingTowardsEnemyGoalAreaStrategyState(GameStrategyInfo gameStrategyInfo) : base(
            StateTransitionType.Triggered, gameStrategyInfo)
        {
            Transitions = new Transition[]
            {
                new IsPlayerBlockedStrategyTransition(GameStrategyInfo, this),
                new NearEnemyGoalAreaTransition(GameStrategyInfo),
                new FarFromEnemyGoalAreaTransition(GameStrategyInfo)
            };
        }
    }
}