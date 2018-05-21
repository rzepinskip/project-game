using PlayerStateCoordinator.Common;
using PlayerStateCoordinator.Common.Transitions;
using PlayerStateCoordinator.NormalPlayer.Transitions;

namespace PlayerStateCoordinator.NormalPlayer.States
{
    public class MoveToPieceStrategyState : NormalPlayerStrategyState
    {
        public MoveToPieceStrategyState(NormalPlayerStrategyInfo playerStrategyInfo) : base(
            StateTransitionType.Triggered,
            playerStrategyInfo)
        {
            Transitions = new Transition[]
            {
                new IsPlayerBlockedStrategyTransition(playerStrategyInfo, this),
                new IsOnFieldWithNoPieceStrategyTransition(playerStrategyInfo),
                new IsNoPieceAvailableStrategyTransition(playerStrategyInfo),
                new IsOnFieldWithPieceStrategyTransition(playerStrategyInfo)
            };
        }
    }
}