using System;
using System.Collections.Generic;
using Common.Interfaces;
using PlayerStateCoordinator.Common.States;

namespace PlayerStateCoordinator.Common.Transitions
{
    public class ErrorTransition : Transition
    {
        public ErrorTransition(State nextState)
        {
            _nextState = nextState;
        }

        private readonly State _nextState;
        public override State NextState => _nextState.Copy();

        public override IEnumerable<IMessage> Message => new List<IMessage>();

        public override bool IsPossible()
        {
            return true;
        }
    }
}