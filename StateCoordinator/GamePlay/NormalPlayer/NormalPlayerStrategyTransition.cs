namespace PlayerStateCoordinator.GamePlay.NormalPlayer
{
    public abstract class NormalPlayerStrategyTransition : GameStrategyTransition
    {
        protected NormalPlayerStrategyInfo NormalPlayerStrategyInfo;

        protected NormalPlayerStrategyTransition(NormalPlayerStrategyInfo normalPlayerStrategyInfo) : base(
            normalPlayerStrategyInfo)
        {
            NormalPlayerStrategyInfo = normalPlayerStrategyInfo;
        }
    }
}