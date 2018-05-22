using PlayerStateCoordinator.Common;
using PlayerStateCoordinator.Common.Transitions;
using PlayerStateCoordinator.GamePlay.NormalPlayer.Transitions;

namespace PlayerStateCoordinator.GamePlay.NormalPlayer.States
{
    public class MoveToPieceStrategyState : NormalPlayerStrategyState
    {
        public MoveToPieceStrategyState(NormalPlayerStrategyInfo playerStrategyInfo) : base(
            StateTransitionType.Triggered,
            playerStrategyInfo)
        {
            RestrictedToTaskArea = true;
            Transitions = new Transition[]
            {
                new IsNormalPlayerBlockedTransition(playerStrategyInfo, this),
                new IsOnFieldWithNoPieceStrategyTransition(playerStrategyInfo),
                new IsNoPieceAvailableStrategyTransition(playerStrategyInfo),
                new IsOnFieldWithPieceStrategyTransition(playerStrategyInfo)
            };
        }
    }
}