using System.Collections.Generic;
using PlayerStateCoordinator.Info;
using PlayerStateCoordinator.States;
using PlayerStateCoordinator.TeamLeader.Transitions;
using PlayerStateCoordinator.Transitions;

namespace PlayerStateCoordinator.TeamLeader.States
{
    public class InitLeaderStrategyState : NormalPlayerStrategyState
    {
        public InitLeaderStrategyState(GameStrategyInfo gameStrategyInfo) : base(StateTransitionType.Immediate, gameStrategyInfo)
        {
            Transitions = new Transition [] 
            {
                new NearEnemyGoalAreaTransition(gameStrategyInfo),
                new FarFromEnemyGoalAreaTransition(GameStrategyInfo), 
            };
        }
    }

}