using PlayerStateCoordinator.Info;
using PlayerStateCoordinator.States;
using PlayerStateCoordinator.TeamLeader.Transitions;
using PlayerStateCoordinator.Transitions;
using PlayerStateCoordinator.Transitions.GameStrategyTransitions;

namespace PlayerStateCoordinator.TeamLeader.States
{
    public class MovingTowardsEnemyGoalAreaStrategyState : NormalPlayerStrategyState
    {
        public MovingTowardsEnemyGoalAreaStrategyState(GameStrategyInfo gameStrategyInfo) : base(StateTransitionType.Triggered, gameStrategyInfo)
        {
            Transitions = new Transition []{
                new IsPlayerBlockedStrategyTransition(GameStrategyInfo, this), 
                new NearEnemyGoalAreaTransition(GameStrategyInfo),
                new FarFromEnemyGoalAreaTransition(GameStrategyInfo)
            };
        }
    }
}