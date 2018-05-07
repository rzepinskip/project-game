using Player.Strategy.Conditions.GameConditions;
using Player.Strategy.StateInfo;

namespace Player.Strategy.States.GameStates
{
    internal class GameStartedState : GameInitState
    {
        public GameStartedState(GameStateInfo gameStateInfo) : base(gameStateInfo)
        {
            if (gameStateInfo.PlayerStrategyFactory != null)
                gameStateInfo.CreatePlayerStrategy();
            gameStateInfo.IsRunning = true;
            transitionConditions.Add(new HasGameStartedCondition(gameStateInfo));
            transitionConditions.Add(new HasGameEndedCondition(gameStateInfo));
        }
    }
}