using Common.BoardObjects;
using Messaging.Requests;

namespace Player.Strategy
{
    public interface IStrategy
    {
        Request NextMove(Location location);
    }
}