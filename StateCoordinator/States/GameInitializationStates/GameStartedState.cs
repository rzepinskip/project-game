using PlayerStateCoordinator.Info;
using PlayerStateCoordinator.Transitions;
using PlayerStateCoordinator.Transitions.GameInitializationTransitions;

namespace PlayerStateCoordinator.States.GameInitializationStates
{
    public class GameStartedState : GameInitializationState
    {
        public GameStartedState(GameInitializationInfo baseInfo) : base(
            StateTransitionType.Triggered,
            new Transition[]
            {
                new GameStartedTransition(baseInfo),
                new GameEndedTransition(baseInfo)
            },
            baseInfo)
        {
            //if (gameInitializationInfo.PlayerStrategyFactory != null)
            //    gameInitializationInfo.CreatePlayerStrategy();
            baseInfo.IsGameRunning = true;
        }
    }
}