using Player.Interfaces;

namespace Player.Strategy
{
    public class PlayerStrategyFactory : IPlayerStrategyFactory
    {
        private readonly Player _player;

        public PlayerStrategyFactory(Player player)
        {
            _player = player;
        }

        public IStrategy CreatePlayerStrategy()
        {
            return new PlayerStrategy(_player.PlayerBoard, _player, _player.PlayerGuid, _player.GameId);
        }
    }
}