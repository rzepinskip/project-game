using System;
using System.Collections.Generic;
using System.Text;
using Player.Strategy.Conditions.GameConditions;
using Player.Strategy.StateInfo;

namespace Player.Strategy.States.GameStates
{
    public class GetGamesState : GameInitState
    {
        public GetGamesState(GameStateInfo gameStateInfo)
            : base(gameStateInfo)
        {
            transitionConditions.Add(new GetGamesCondition(gameStateInfo));
        }


    }
}
