using System;
using System.Collections.Generic;
using Common.Interfaces;
using PlayerStateCoordinator.Info;
using PlayerStateCoordinator.Transitions;

namespace PlayerStateCoordinator.States
{
    public class GameStrategyState : State
    {
        private readonly GameStrategyInfo _gameStrategyInfo;

        public GameStrategyState(StateTransitionType transitionType, IEnumerable<Transition> transitions, GameStrategyInfo gameStrategyInfo) : base(transitionType, transitions, gameStrategyInfo)
        {
            _gameStrategyInfo = gameStrategyInfo;
        }

        protected override Transition HandleRequestMessage(IRequestMessage requestMessage)
        {
           throw new NotImplementedException();
        }

        protected override Transition HandleErrorMessage(IErrorMessage errorMessage)
        {
            return new ErrorTransition(_gameStrategyInfo.PlayerGameStrategyBeginningState);
        }
    }
}
