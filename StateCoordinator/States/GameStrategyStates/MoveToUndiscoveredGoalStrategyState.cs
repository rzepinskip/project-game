using PlayerStateCoordinator.Info;
using PlayerStateCoordinator.Transitions;
using PlayerStateCoordinator.Transitions.GameStrategyTransitions;

namespace PlayerStateCoordinator.States.GameStrategyStates
{
    public class MoveToUndiscoveredGoalStrategyState : GameStrategyState
    {
        public MoveToUndiscoveredGoalStrategyState(GameStrategyInfo gameStrategyInfo) : base(
            StateTransitionType.Triggered,
            gameStrategyInfo)
        {
            Transitions = new Transition[]
            {
                new IsPlayerBlockedStrategyTransition(gameStrategyInfo, this),
                new IsOnUndiscoveredGoalFieldStrategyTransition(gameStrategyInfo),
                new HasNormalPieceStrategyTransition(gameStrategyInfo),
            };
        }
    }
}