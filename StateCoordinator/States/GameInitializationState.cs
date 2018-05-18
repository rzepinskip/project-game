﻿using System.Collections.Generic;
using Common.Interfaces;
using PlayerStateCoordinator.Info;
using PlayerStateCoordinator.Transitions;

namespace PlayerStateCoordinator.States
{
    public class GameInitializationState : State
    {
        private readonly GameInitializationInfo _gameInitializationInfo;

        public GameInitializationState(StateTransitionType transitionType, IEnumerable<Transition> transitions,
            GameInitializationInfo gameInitializationInfo) : base(transitionType, transitions)
        {
            _gameInitializationInfo = gameInitializationInfo;
        }

        protected override Transition HandleErrorMessage(IErrorMessage errorMessage)
        {
            return new ErrorTransition(_gameInitializationInfo.PlayerGameInitializationBeginningState);
        }
    }
}