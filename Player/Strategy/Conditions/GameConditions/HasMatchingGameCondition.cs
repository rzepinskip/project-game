﻿using System.Linq;
using Common.Interfaces;
using Messaging.InitializationMessages;
using Player.Strategy.StateInfo;
using Player.Strategy.States;
using Player.Strategy.States.GameStates;

namespace Player.Strategy.Conditions.GameConditions
{
    public class HasMatchingGame : GameCondition
    {
        public HasMatchingGame(GameStateInfo gameStateInfo) : base(gameStateInfo)
        {
        }

        public override bool CheckCondition()
        {
            if (GameStateInfo.GameInfo == null)
                return false;
            return GameStateInfo.GameInfo.Any(x => x.GameName == GameStateInfo.GameName);
        }

        public override BaseState GetNextState(BaseState fromStrategyState)
        {
            return new AwaitingJoinResponseState(GameStateInfo);
        }

        public override IMessage GetNextMessage(BaseState fromStrategyState)
        {
            return new JoinGameMessage(GameStateInfo.GameName, GameStateInfo.Role, GameStateInfo.Color);
        }

        public override bool ReturnsMessage(BaseState fromStrategyState)
        {
            return true;
        }
    }
}