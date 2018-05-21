using PlayerStateCoordinator.Common;
using PlayerStateCoordinator.Common.States;

namespace PlayerStateCoordinator.GamePlay.TeamLeader
{
    public class LeaderStrategyState : GamePlayStrategyState
    {
        public LeaderStrategyState(StateTransitionType transitionType,
            LeaderStrategyInfo playerStrategyInfo) : base(transitionType, playerStrategyInfo)
        {
        }

        protected override bool IsExchangeWantedWithPlayer(int initiatorId)
        {
            return false;
        }
    }
}