﻿using System.Collections.Generic;
using Common.Interfaces;
using Messaging.Requests;
using PlayerStateCoordinator.Info;
using PlayerStateCoordinator.States;
using PlayerStateCoordinator.States.GameStrategyStates;

namespace PlayerStateCoordinator.Transitions.GameStrategyTransitions
{
    public class HasNoPieceStrategyTransition : GameStrategyTransition
    {
        public HasNoPieceStrategyTransition(GameStrategyInfo gameStrategyInfo) : base(gameStrategyInfo)
        {
        }

        public override State NextState => new DiscoverStrategyState(GameStrategyInfo);

        public override IEnumerable<IMessage> Message => new List<IMessage> {new DiscoverRequest(GameStrategyInfo.PlayerGuid, GameStrategyInfo.GameId)};

        public override bool IsPossible()
        {
            var playerInfo = GameStrategyInfo.Board.Players[GameStrategyInfo.PlayerId];
            return playerInfo.Piece == null;
        }
    }
}