using PlayerStateCoordinator.Common;
using PlayerStateCoordinator.Common.Transitions;
using PlayerStateCoordinator.NormalPlayer.Transitions;

namespace PlayerStateCoordinator.NormalPlayer.States
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