using Player.Strategy;

namespace Player.Interfaces
{
    public interface IPlayerStrategyFactory
    {
        IStrategy CreatePlayerStrategy();
    }
}