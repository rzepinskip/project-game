
namespace PlayerStateCoordinator.Common.Transitions
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