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

        protected override GamePlayStrategyState GetRecoveryFromBlockedState()
        {
            return new DiscoverStrategyState(_normalPlayerStrategyInfo);
        }

        protected override GamePlayStrategyState GetFromState()
        {
            return Activator.CreateInstance(FromState.GetType(),
                GamePlayStrategyInfo) as NormalPlayerStrategyState;
        }

        protected override bool IsFromStateOnlyInTaskArea(GamePlayStrategyState fromState)
        {
            var onlyTaskArea = false;
            switch (FromState)
            {
                case MoveToPieceStrategyState _:
                {
                    onlyTaskArea = true;
                    break;
                }
                case InGoalAreaMovingToTaskStrategyState _:
                case MoveToUndiscoveredGoalStrategyState _:
                case InitialMoveAfterPlaceStrategyState _:
                    break;
                default:
                    Console.WriteLine("Unexpeted state in PlayerBlocked transition");
                    break;
            }

            return onlyTaskArea;
        }
    }
}