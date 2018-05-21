using System;
using PlayerStateCoordinator.Common;
using PlayerStateCoordinator.Common.States;
using PlayerStateCoordinator.Common.Transitions;
using PlayerStateCoordinator.TeamLeader.States;

namespace PlayerStateCoordinator.TeamLeader.Transitions
{
    public class IsLeaderBlockedTransition : IsPlayerBlockedTransition
    {
        private new readonly LeaderStrategyState _fromState;
        private readonly LeaderStrategyInfo _leaderStrategyInfo;
        public IsLeaderBlockedTransition(LeaderStrategyInfo gamePlayStrategyInfo, LeaderStrategyState fromState) : base(gamePlayStrategyInfo, fromState)
        {
            _fromState = fromState;
            _leaderStrategyInfo = gamePlayStrategyInfo;
        }

        protected override GamePlayStrategyState GetRecoveryFromBlockedState()
        {
            return new MovingTowardsEnemyGoalAreaStrategyState(_leaderStrategyInfo);
        }

        protected override GamePlayStrategyState GetFromState()
        {

            Console.WriteLine("Recognized leader state");
            return Activator.CreateInstance(_fromState.GetType(),
                GamePlayStrategyInfo) as LeaderStrategyState;

        }
    }
}