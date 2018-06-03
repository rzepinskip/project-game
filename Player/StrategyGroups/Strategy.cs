using PlayerStateCoordinator.Common.States;

namespace Player.StrategyGroups
{
    public abstract class Strategy
    {
        public abstract State GetBeginningState();
    }
}