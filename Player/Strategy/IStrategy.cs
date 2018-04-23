using Common.Interfaces;
using Player.Strategy.States;

namespace Player.Strategy
{
    public interface IStrategy
    {
        IMessage NextMove();
        BaseState CurrentStrategyState { get; set; }
        bool StrategyReturnsMessage();
    }
}