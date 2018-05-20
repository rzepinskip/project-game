using PlayerStateCoordinator.Info;
using PlayerStateCoordinator.Transitions;
using PlayerStateCoordinator.Transitions.GameStrategyTransitions;

namespace PlayerStateCoordinator.States.GameStrategyStates
{
    public class MoveToPieceStrategyState : NormalPlayerStrategyState
    {
        public MoveToPieceStrategyState(GameStrategyInfo gameStrategyInfo) : base(
            StateTransitionType.Triggered,
            gameStrategyInfo)
        {
            Transitions = new Transition[]
            {
                new IsPlayerBlockedStrategyTransition(gameStrategyInfo, this),
                new IsOnFieldWithNoPieceStrategyTransition(gameStrategyInfo),
                new IsNoPieceAvailableStrategyTransition(gameStrategyInfo),
                new IsOnFieldWithPieceStrategyTransition(gameStrategyInfo)
            };
        }
    }
}