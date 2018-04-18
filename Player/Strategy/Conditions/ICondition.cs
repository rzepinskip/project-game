using Messaging.Requests;
using Player.Strategy.States;
using Player.Strategy.States.StrategyStates;

namespace Player.Strategy.Conditions
{
    public interface ICondition
    {
        bool CheckCondition();
        StrategyState GetNextState(StrategyState fromStrategyState);
        Request GetNextMessage(StrategyState fromStrategyState);
    }
}