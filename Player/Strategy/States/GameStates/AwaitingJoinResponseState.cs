using Player.Strategy.Conditions.GameConditions;
using Player.Strategy.StateInfo;

namespace Player.Strategy.States.GameStates
{
    public class AwaitingJoinResponseState : GameInitState
    {
        public AwaitingJoinResponseState(GameStateInfo gameStateInfo)
        {
            transitionConditions.Add(new IsJoinSuccessfulCondition(gameStateInfo));
            transitionConditions.Add(new IsJoinUnsuccessfulCondition(gameStateInfo));
        }
    }
}