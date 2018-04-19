using System;
using System.Collections.Generic;
using System.Text;
using Player.Strategy.StateInfo;

namespace Player.Strategy.States.GameStates
{
    public class MatchingGameState : GameInitState
    {
        public MatchingGameState(GameStateInfo gameStateInfo) : base(gameStateInfo)
        {
            transitionConditions.Add(new HasMatchingGame(gameStateInfo));
            transitionConditions.Add(new HasNoMatchingGame(gameStateInfo));
        }
    }
}
