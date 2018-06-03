using PlayerStateCoordinator.Common;
using PlayerStateCoordinator.Common.Transitions;
using PlayerStateCoordinator.GamePlay.NormalPlayer.Transitions;

namespace PlayerStateCoordinator.GamePlay.NormalPlayer.States
{
    public class PickupPieceStrategyState : NormalPlayerStrategyState
    {
        public PickupPieceStrategyState(NormalPlayerStrategyInfo playerStrategyInfo) : base(
            StateTransitionType.Triggered,
            playerStrategyInfo)
        {
            Transitions = new Transition[]
            {
                new IsAbleToTestStrategyTransition(playerStrategyInfo)
            };
        }
    }
}