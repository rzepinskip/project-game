using System;
using System.Collections.Generic;
using System.Text;
using Player.Strategy.Conditions;
using Player.Strategy.StateInfo;
using Player.Strategy;

namespace Player.Strategy.States
{
    public abstract class BaseState
    {
        protected List<ICondition> transitionConditions;
        protected BaseInfo stateInfo;
    }
}
