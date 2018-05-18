using System;
using System.Collections.Generic;
using Common.Interfaces;
using PlayerStateCoordinator.Info;
using PlayerStateCoordinator.States;

namespace PlayerStateCoordinator.Transitions.GameInitializationTransitions
{
    public class GameStartedTransition : GameInitializationTransition
    {
        public GameStartedTransition(GameInitializationInfo gameInitializationInfo)
            : base(gameInitializationInfo)
        {
        }

        //public override bool CheckTransition()
        //{
        //    return GameInitializationInfo.IsRunning;
        //}

        //public override BaseState GetNextState(BaseState fromStrategyState)
        //{
        //    return fromStrategyState;
        //}

        //public override IMessage GetNextMessage(BaseState fromStrategyState)
        //{
        //    return GameInitializationInfo.PlayerStrategy.NextMove();
        //}

        //public override bool ReturnsMessage(BaseState fromStrategyState)
        //{
        //    return GameInitializationInfo.PlayerStrategy.StrategyReturnsMessage();
        //}

        public override State NextState => throw new NotImplementedException();
        public override IEnumerable<IMessage> Message => throw new NotImplementedException();

        public override bool IsPossible()
        {
            return GameInitializationInfo.IsGameRunning;
        }
    }
}