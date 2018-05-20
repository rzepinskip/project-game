using System;
using System.Collections.Generic;
using Common.Interfaces;
using PlayerStateCoordinator.Common.Transitions;
using PlayerStateCoordinator.Info;
using PlayerStateCoordinator.Common.States;

namespace PlayerStateCoordinator.Common.States
{
    public abstract class State
    {
        private readonly MessageHandler _messageHandler;
        public readonly BaseInfo Info;
        public readonly StateTransitionType TransitionType;
        protected IEnumerable<Transition> Transitions;

        protected State(StateTransitionType transitionType, BaseInfo info)
        {
            TransitionType = transitionType;
            Transitions = new Transition[0];
            Info = info;
            _messageHandler = new MessageHandler(HandleRequestMessage, HandleResponseMessage, HandleErrorMessage, HandleGenericMessage);
        }

        public Transition Process(IMessage message)
        {
            return _messageHandler.Process(message);
        }

        private Transition ProceedToNextState()
        {
            foreach (var transition in Transitions)
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

        protected virtual Transition HandleGenericMessage(IMessage genericMessage)
        {
            return ProceedToNextState();
        }

        protected virtual Transition HandleNoMatchingTransitionCase()
        {
            throw new NotImplementedException();
        }
    }
}