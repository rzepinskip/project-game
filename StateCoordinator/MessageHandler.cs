using System;
using Common.Interfaces;
using PlayerStateCoordinator.Common.Transitions;

namespace PlayerStateCoordinator
{
    public class MessageHandler
    {
        private readonly Func<IErrorMessage, Transition> _errorMessageHandler;
        private readonly Func<IMessage, Transition> _genericMessageHandler;
        private readonly Func<IRequestMessage, Transition> _requestMessageHandler;
        private readonly Func<IResponseMessage, Transition> _responseMessageHandler;


        public MessageHandler(Func<IRequestMessage, Transition> requestMessageHandler,
            Func<IResponseMessage, Transition> responseMessageHandler,
            Func<IErrorMessage, Transition> errorMessageHandler,
            Func<IMessage, Transition> genericMessageHandler)
        {
            _requestMessageHandler = requestMessageHandler;
            _responseMessageHandler = responseMessageHandler;
            _errorMessageHandler = errorMessageHandler;
            _genericMessageHandler = genericMessageHandler;
        }

        public Transition Process(IMessage message)
        {
            return Handle(message as dynamic);
        }

        private Transition Handle(IMessage message)
        {
            return _genericMessageHandler(message);
        }

        private Transition Handle(IRequestMessage requestMessage)
        {
            return _requestMessageHandler(requestMessage);
        }

        private Transition Handle(IResponseMessage responseMessage)
        {
            return _responseMessageHandler(responseMessage);
        }

        private Transition Handle(IErrorMessage errorMessage)
        {
            return _errorMessageHandler(errorMessage);
        }
    }
}