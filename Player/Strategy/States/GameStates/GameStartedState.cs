using Player.Strategy.Conditions.GameConditions;
using Player.Strategy.StateInfo;
using Player.Strategy.States;

namespace Player.Strategy.States.GameStates
{
    internal class GameStartedState : GameInitState
    {
        public GameStartedState(GameStateInfo gameStateInfo) : base(gameStateInfo)
        {
            gameStateInfo.CreatePlayerStrategy();
            gameStateInfo.IsRunning = true;
            transitionConditions.Add(new HasGameStartedCondition(gameStateInfo));
            transitionConditions.Add(new HasGameEndedCondition(gameStateInfo));
        }
    }
}