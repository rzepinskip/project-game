using Common.BoardObjects;
using Messaging.Requests;
using Player.Strategy.States;
using Player.Strategy.States.StrategyStates;

namespace Player.Strategy
{
    public interface IStrategy
    {
        Request NextMove(Location location);
        StrategyState CurrentStrategyState { get; set; }
    }
}