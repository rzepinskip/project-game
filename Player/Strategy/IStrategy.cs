using Shared.BoardObjects;
using Shared.GameMessages;

namespace Player.Strategy
{
    public interface IStrategy
    {
        GameMessage NextMove(Location location);
    }
}