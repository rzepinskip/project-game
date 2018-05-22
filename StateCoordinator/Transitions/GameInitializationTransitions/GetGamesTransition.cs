﻿using System.Collections.Generic;
using Common.Interfaces;
using Messaging.InitializationMessages;
using PlayerStateCoordinator.Info;
using PlayerStateCoordinator.States;
using PlayerStateCoordinator.States.GameInitializationStates;

namespace PlayerStateCoordinator.Transitions.GameInitializationTransitions
{
    public class GetGamesTransition : GameInitializationTransition
    {
        public GetGamesTransition(GameInitializationInfo gameInitializationInfo) : base(gameInitializationInfo)
        {
        }

        public override State NextState => new MatchingGameState(GameInitializationInfo);

        public override IEnumerable<IMessage> Message => new List<IMessage> {new GetGamesMessage()};

        public override bool IsPossible()
        {
            return true;
        }
    }
}