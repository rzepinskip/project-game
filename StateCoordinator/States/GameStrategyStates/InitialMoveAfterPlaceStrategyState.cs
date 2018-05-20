using PlayerStateCoordinator.Info;
using PlayerStateCoordinator.Transitions;
using PlayerStateCoordinator.Transitions.GameStrategyTransitions;

namespace PlayerStateCoordinator.States.GameStrategyStates
{
    /// <summary>
    ///     Send Move request after initiating KnowledgeExchange to ensure immmunity to lack of response from KnowledgeExchange
    ///     subject(f.e. due to disconnection)
    /// </summary>
    public class InitialMoveAfterPlaceStrategyState : NormalPlayerStrategyState
    {
        public InitialMoveAfterPlaceStrategyState(GameStrategyInfo gameStrategyInfo) : base(
            StateTransitionType.Immediate,
            gameStrategyInfo)
        {
            Transitions = new Transition[]
            {
                new IsPlayerBlockedStrategyTransition(gameStrategyInfo, new InGoalAreaMovingToTaskStrategyState(GameStrategyInfo)),
                new IsInGoalWithoutPieceStrategyTransition(gameStrategyInfo)
            };
        }
    }
}