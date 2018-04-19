using System;
using System.Collections.Generic;
using System.Text;
using Player.Strategy;

namespace Player.Interfaces
{
    public interface IPlayerStrategyFactory
    {
        IStrategy CreatePlayerStrategy();
    }
}
