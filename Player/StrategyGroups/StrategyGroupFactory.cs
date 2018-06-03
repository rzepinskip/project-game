using System;
using Player.StrategyGroups.Advanced;
using Player.StrategyGroups.Basic;

namespace Player.StrategyGroups
{
    public class StrategyGroupFactory
    {
        public static StrategyGroup Create(StrategyGroupType groupType)
        {
            switch (groupType)
            {
                case StrategyGroupType.Basic:
                    return new BasicStrategyGroup();
                case StrategyGroupType.Advanced:
                    return new AdvancedStrategyGroup();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}