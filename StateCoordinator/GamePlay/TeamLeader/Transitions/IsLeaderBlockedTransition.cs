using System;
using PlayerStateCoordinator.GamePlay.TeamLeader.States;

namespace PlayerStateCoordinator.GamePlay.TeamLeader.Transitions
{
    public class IsLeaderBlockedTransition : IsPlayerBlockedTransition
    {
        private readonly LeaderStrategyInfo _leaderStrategyInfo;

        public IsLeaderBlockedTransition(LeaderStrategyInfo gamePlayStrategyInfo, LeaderStrategyState fromState) : base(
            gamePlayStrategyInfo, fromState)
        {
            _leaderStrategyInfo = gamePlayStrategyInfo;
        }

        protected override GamePlayStrategyState GetRecoveryFromBlockedState()
        {
            return new MovingTowardsEnemyGoalAreaStrategyState(_leaderStrategyInfo);
        }
        protected override void CheckIfFromStateIsPredicted(GamePlayStrategyState fromState)
        {
            switch (FromState)
            {
                    case MovingTowardsEnemyGoalAreaStrategyState _:
                        break;
                    default:
                        Console.WriteLine($"Unexpeted state in {this.GetType().Name} transition");
                        break;
            }
        
        }
    }
}