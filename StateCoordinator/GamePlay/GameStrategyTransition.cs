using PlayerStateCoordinator.Common;
using PlayerStateCoordinator.Common.Transitions;

namespace PlayerStateCoordinator.GamePlay
{
    public abstract class GameStrategyTransition : Transition
    {
        protected readonly GamePlayStrategyInfo GamePlayStrategyInfo;

        protected GameStrategyTransition(GamePlayStrategyInfo leaderStrategyInfo)
        {
            GamePlayStrategyInfo = leaderStrategyInfo;
        }
    }
}