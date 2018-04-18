using Common.Interfaces;
using Messaging.Requests;
using Player.Strategy.States;
using Player.Strategy.States.StrategyStates;

namespace Player.Strategy.Conditions
{
    public interface ICondition
    {
        bool CheckCondition();
        BaseState GetNextState(BaseState fromStrategyState);
        IMessage GetNextMessage(BaseState fromStrategyState);
    }
}