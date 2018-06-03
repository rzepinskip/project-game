using System;
using PlayerStateCoordinator.GamePlay.NormalPlayer.States;

namespace PlayerStateCoordinator.GamePlay.NormalPlayer.Transitions
{
    public class IsNormalPlayerBlockedTransition : IsPlayerBlockedTransition
    {
        private readonly NormalPlayerStrategyInfo _normalPlayerStrategyInfo;

        public IsNormalPlayerBlockedTransition(NormalPlayerStrategyInfo gamePlayStrategyInfo,
            NormalPlayerStrategyState fromState) : base(gamePlayStrategyInfo, fromState)
        {
            _normalPlayerStrategyInfo = gamePlayStrategyInfo;
        }

        protected override GamePlayStrategyState NextStateForFullyBlockedCase => new DiscoverStrategyState(_normalPlayerStrategyInfo);


        protected override void CheckIfFromStateIsPredicted(GamePlayStrategyState fromState)
        {
            switch (FromState)
            {
                case MoveToPieceStrategyState _:
                case InGoalAreaMovingToTaskStrategyState _:
                case MoveToUndiscoveredGoalStrategyState _:
                case InitialMoveAfterPlaceStrategyState _:
                    break;
                default:
                    Console.WriteLine($"Unexpeted state in {this.GetType().Name} transition");
                    break;
            }
        }
    }
}