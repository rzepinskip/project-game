using System.Collections.Generic;
using Common.Interfaces;
using PlayerStateCoordinator.Common.States;

namespace PlayerStateCoordinator.Common.Transitions
{
    public abstract class Transition
    {
        public abstract State NextState { get; }

        /// <summary>
        ///     Returns 0 or 1 message
        /// </summary>
        public abstract IEnumerable<IMessage> Message { get; }

        public abstract bool IsPossible();
    }
}