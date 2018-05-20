using Common.Interfaces;
using PlayerStateCoordinator.Common;
using PlayerStateCoordinator.Common.States;
using PlayerStateCoordinator.Common.Transitions;
using PlayerStateCoordinator.Info;

namespace PlayerStateCoordinator.GameInitialization
{
    public abstract class GameInitializationState : State
    {
        private readonly GameInitializationInfo _gameInitializationInfo;

        protected GameInitializationState(StateTransitionType transitionType,
            GameInitializationInfo gameInitializationInfo) : base(transitionType, gameInitializationInfo)
        {
            _gameInitializationInfo = gameInitializationInfo;
        }

        protected override Transition HandleErrorMessage(IErrorMessage errorMessage)
        {
            return new ErrorTransition(_gameInitializationInfo.PlayerGameInitializationBeginningState);
        }
    }
}