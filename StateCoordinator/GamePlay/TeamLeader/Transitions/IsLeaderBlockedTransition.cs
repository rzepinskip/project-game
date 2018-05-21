using System;
using PlayerStateCoordinator.Common.States;
using PlayerStateCoordinator.Common.Transitions;
using PlayerStateCoordinator.GamePlay.TeamLeader.States;

namespace PlayerStateCoordinator.GamePlay.TeamLeader.Transitions
{
    public class IsLeaderBlockedTransition : IsPlayerBlockedTransition
    {
        private readonly LeaderStrategyInfo _leaderStrategyInfo;
        public IsLeaderBlockedTransition(LeaderStrategyInfo gamePlayStrategyInfo, LeaderStrategyState fromState) : base(gamePlayStrategyInfo, fromState)
        {
            _leaderStrategyInfo = gamePlayStrategyInfo;
        }

        protected override GamePlayStrategyState GetRecoveryFromBlockedState()
        {
            return new MovingTowardsEnemyGoalAreaStrategyState(_leaderStrategyInfo);
        }

        protected override GamePlayStrategyState GetFromState()
        {

            Console.WriteLine("Recognized leader state");
            return Activator.CreateInstance(FromState.GetType(),
                GamePlayStrategyInfo) as LeaderStrategyState;

        }
    }
}