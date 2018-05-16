using Common.Interfaces;
using Player.Strategy.States;

namespace Player.Strategy
{
    public interface IStrategy
    {
        BaseState CurrentGameState { get; set; }
        IMessage NextMove();
        bool StrategyReturnsMessage();
    }
}