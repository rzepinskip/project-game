using System;
using System.Collections.Generic;
using Common.Interfaces;
using PlayerStateCoordinator.Common.States;

namespace PlayerStateCoordinator.Common.Transitions
{
    public class LoopbackTransition : Transition
    {
        public LoopbackTransition(State nextState, IEnumerable<IMessage> message)
        {
            _nextState = nextState;
            Message = message;
        }

        private readonly State _nextState;
        public override State NextState => _nextState.Copy();

        public override IEnumerable<IMessage> Message { get; }

        public override bool IsPossible()
        {
            return true;
        }
    }
}