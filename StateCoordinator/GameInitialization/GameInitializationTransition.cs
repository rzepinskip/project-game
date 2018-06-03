using PlayerStateCoordinator.Common.Transitions;

namespace PlayerStateCoordinator.GameInitialization
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