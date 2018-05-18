using PlayerStateCoordinator.Info;

namespace PlayerStateCoordinator.Transitions
{
    public abstract class GameStrategyTransition : Transition
    {
        protected readonly GameStrategyInfo GameStrategyInfo;

        protected GameStrategyTransition(GameStrategyInfo gameStrategyInfo)
        {
            GameStrategyInfo = gameStrategyInfo;
        }
    }
}
