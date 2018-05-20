using System.Collections.Generic;
using Common.Interfaces;
using PlayerStateCoordinator.Common.States;

namespace PlayerStateCoordinator.Common.Transitions
{
    public class ErrorTransition : Transition
    {
        public ErrorTransition(State nextState)
        {
            NextState = nextState;
        }

        public override State NextState { get; }

        public override IEnumerable<IMessage> Message => new List<IMessage>();

        public override bool IsPossible()
        {
            return true;
        }
    }
}