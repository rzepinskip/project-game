using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Common;
using Common.Interfaces;
using Messaging.InitialisationMessages;
using Player.Strategy.StateInfo;
using Player.Strategy.States;
using Player.Strategy.States.GameStates;

namespace Player.Strategy.Conditions.GameConditions
{
    public class HasNoMatchingGame : GameCondition
    {

        public HasNoMatchingGame(GameStateInfo gameStateInfo) : base(gameStateInfo)
        {
        }

        public override bool CheckCondition()
        {
            return GameStateInfo.GameInfo.All(x => x.GameName != GameStateInfo.GameName);
        }

        public override BaseState GetNextState(BaseState fromStrategyState)
        {
            return new GetGamesState(GameStateInfo);
        }

        public override IMessage GetNextMessage(BaseState fromStrategyState)
        {
            //Thread Sleep to avoid CS spamming
            Thread.Sleep(1000);
            return new GetGamesMessage();
        }
    }
}
