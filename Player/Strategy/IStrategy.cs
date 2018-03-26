using Common.BoardObjects;
using Messaging.Requests;
using Player.Strategy.States;

namespace Player.Strategy
{
    public interface IStrategy
    {
        Request NextMove(Location location);
        State CurrentState { get; set; }
    }
}