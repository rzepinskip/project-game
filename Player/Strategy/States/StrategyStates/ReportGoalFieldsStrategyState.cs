using System;
using System.Collections.Generic;
using System.Text;
using Player.Strategy.Conditions.StrategyConditions;
using Player.Strategy.StateInfo;

namespace Player.Strategy.States.StrategyStates
{
    class ReportGoalFieldsStrategyState : StrategyState
    {
        public ReportGoalFieldsStrategyState(StrategyInfo strategyInfo) : base(strategyInfo)
        {
            transitionConditions.Add(new IsThereSomeoneToCommunicateWithStrategyCondition(strategyInfo));
            transitionConditions.Add(new IsInGoalWithoutPieceStrategyCondition(strategyInfo));
        }
    }
}
