using PlayerStateCoordinator.Common;
using PlayerStateCoordinator.Common.States;

namespace PlayerStateCoordinator.TeamLeader
{
    public class LeaderStrategyState : GamePlayStrategyState
    {
        public LeaderStrategyState(StateTransitionType transitionType,
            GameStrategyInfo gameStrategyInfo) : base(transitionType, gameStrategyInfo)
        {
        }

        protected override bool IsExchangeWantedWithPlayer(int initiatorId)
        {
            return false;
        }
    }
}