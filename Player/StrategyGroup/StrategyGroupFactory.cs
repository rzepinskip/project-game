using System;

namespace Player.StrategyGroup
{
    public class StrategyGroupFactory
    {
        public StrategyGroup Create(StrategyGroupType groupType)
        {
            switch (groupType)
            {
                case StrategyGroupType.Primitive:
                    return new PrimitiveStrategyGroup();
                case StrategyGroupType.Advanced:
                    return new AdvancedStrategyGroup();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}