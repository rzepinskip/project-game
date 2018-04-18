using Common.BoardObjects;
using Common.Interfaces;
using Messaging.Requests;
using Player.Strategy.States;
using Player.Strategy.States.StrategyStates;

namespace Player.Strategy
{
    public interface IStrategy
    {
        IMessage NextMove();
        BaseState CurrentStrategyState { get; set; }
    }
}