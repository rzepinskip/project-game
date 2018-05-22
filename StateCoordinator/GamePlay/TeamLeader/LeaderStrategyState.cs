using PlayerStateCoordinator.Common;

namespace PlayerStateCoordinator.GamePlay.TeamLeader
{
    public abstract class LeaderStrategyState : GamePlayStrategyState
    {
        protected LeaderStrategyState(StateTransitionType transitionType,
            LeaderStrategyInfo playerStrategyInfo) : base(transitionType, playerStrategyInfo)
        {
        }

        protected override bool IsExchangeWantedWithPlayer(int initiatorId)
        {
            return false;
        }
    }
}