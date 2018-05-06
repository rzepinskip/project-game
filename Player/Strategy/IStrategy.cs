using Common.Interfaces;
using Player.Strategy.States;

namespace Player.Strategy
{
    public interface IStrategy
    {
        BaseState CurrentStrategyState { get; set; }
        IMessage NextMove();
        bool StrategyReturnsMessage();
    }
}