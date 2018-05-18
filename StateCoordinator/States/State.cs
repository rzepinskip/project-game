using System;
using System.Collections.Generic;
using Common.Interfaces;
using PlayerStateCoordinator.Transitions;

namespace PlayerStateCoordinator.States
{
    public abstract class State
    {
        private readonly MessageHandler _messageHandler;
        private readonly IEnumerable<Transition> _transitions;
        public readonly StateTransitionType TransitionType;

        protected State(StateTransitionType transitionType, IEnumerable<Transition> transitions)
        {
            TransitionType = transitionType;
            _transitions = transitions;
            _messageHandler = new MessageHandler(HandleRequestMessage, HandleResponseMessage, HandleErrorMessage);
        }

        public Transition Process(IMessage message)
        {
            return _messageHandler.Process(message);
        }

        private Transition ProceedToNextState()
        {
            foreach (var transition in _transitions)
                if (transition.IsPossible())
                    return transition;

            return HandleNoMatchingTransitionCase();
        }

        protected virtual Transition HandleRequestMessage(IRequestMessage requestMessage)
        {
            throw new NotImplementedException();
        }

        protected virtual Transition HandleResponseMessage(IResponseMessage responseMessage)
        {
            return ProceedToNextState();
        }

        protected virtual Transition HandleErrorMessage(IErrorMessage errorMessage)
        {
            throw new NotImplementedException();
        }

        protected virtual Transition HandleNoMatchingTransitionCase()
        {
            throw new NotImplementedException();
        }
    }
}