﻿using PlayerStateCoordinator.Common;
using PlayerStateCoordinator.Common.States;

namespace PlayerStateCoordinator.NormalPlayer
{
    public class NormalPlayerStrategyState : GamePlayStrategyState
    {
        public NormalPlayerStrategyState(StateTransitionType transitionType,
            NormalPlayerStrategyInfo playerStrategyInfo) : base(transitionType, playerStrategyInfo)
        {
        }
    }
}