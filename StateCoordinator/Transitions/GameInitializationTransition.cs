using PlayerStateCoordinator.Info;

namespace PlayerStateCoordinator.Transitions
{
    public abstract class GameInitializationTransition : Transition
    {
        protected readonly GameInitializationInfo GameInitializationInfo;

        protected GameInitializationTransition(GameInitializationInfo gameInitializationInfo)
        {
            GameInitializationInfo = gameInitializationInfo;
        }
    }
}