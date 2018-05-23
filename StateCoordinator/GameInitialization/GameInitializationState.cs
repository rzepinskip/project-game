using System.Collections.Generic;
using Common.Interfaces;
using PlayerStateCoordinator.Common;
using PlayerStateCoordinator.Common.States;
using PlayerStateCoordinator.Common.Transitions;

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

        protected override Transition HandleGenericMessage(IMessage genericMessage)
        {
            return ProceedToNextState();
        }

        protected override Transition HandleErrorMessage(IErrorMessage errorMessage)
        {
            return new ErrorTransition(_gameInitializationInfo.PlayerGameInitializationBeginningState);
        }

        protected override Transition HandleResponseMessage(IResponseMessage responseMessage)
        {
            if(responseMessage is DataMessage)
                return new LoopbackTransition(this, new List<IMessage>());

            return base.HandleResponseMessage(responseMessage);
        }
    }
}