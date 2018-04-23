using System.Collections.Generic;
using Player.Strategy.Conditions;
using Player.Strategy.StateInfo;

namespace Player.Strategy.States.GameStates
{
    public class GameInitState: BaseState
    {
        protected GameInitState(GameStateInfo gameStateInfo)
        {
            stateInfo = gameStateInfo;
            transitionConditions = new List<ICondition>();
        }

        protected GameInitState()
        {
            transitionConditions = new List<ICondition>();
        }
    }
}