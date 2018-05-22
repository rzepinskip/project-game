using PlayerStateCoordinator.Common;
using PlayerStateCoordinator.Common.Transitions;
using PlayerStateCoordinator.GamePlay.NormalPlayer.Transitions;

namespace PlayerStateCoordinator.GamePlay.NormalPlayer.States
{
    /// <summary>
    ///     Send Move request after initiating KnowledgeExchange to ensure immmunity to lack of response from KnowledgeExchange
    ///     subject(f.e. due to disconnection)
    /// </summary>
    public class InitialMoveAfterPlaceStrategyState : NormalPlayerStrategyState
    {
        public InitialMoveAfterPlaceStrategyState(NormalPlayerStrategyInfo playerStrategyInfo) : base(
            StateTransitionType.Immediate,
            playerStrategyInfo)
        {
            Transitions = new Transition[]
            {
                new IsNormalPlayerBlockedTransition(playerStrategyInfo,
                    new InGoalAreaMovingToTaskStrategyState(playerStrategyInfo)),
                new IsInGoalWithoutPieceStrategyTransition(playerStrategyInfo)
            };
        }
    }
}