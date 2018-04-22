using Common.Interfaces;
using Player.Strategy.States;

namespace Player.Strategy.Conditions
{
    public interface ICondition
    {
        bool CheckCondition();
        BaseState GetNextState(BaseState fromStrategyState);
        IMessage GetNextMessage(BaseState fromStrategyState);
        bool ReturnsMessage(BaseState fromStrategyState);
    }
}