using System.Collections.Generic;
using Common.Interfaces;
using Messaging.Requests;
using PlayerStateCoordinator.Info;
using PlayerStateCoordinator.States;
using PlayerStateCoordinator.States.GameStrategyStates;

namespace PlayerStateCoordinator.Transitions.GameStrategyTransitions
{
    public class IsOnDesiredUndiscoveredGoalFieldStrategyTransition : GameStrategyTransition
    {
        public IsOnDesiredUndiscoveredGoalFieldStrategyTransition(GameStrategyInfo gameStrategyInfo) : base(gameStrategyInfo)
        {
        }

        public override State NextState => new ReportGoalFieldsStrategyState(GameStrategyInfo);

        public override IEnumerable<IMessage> Message
        {
            get
            {
                GameStrategyInfo.UndiscoveredGoalFields.RemoveAt(0);
                return new List<IMessage>
                {
                    new PlacePieceRequest(GameStrategyInfo.PlayerGuid, GameStrategyInfo.GameId)
                };
            }
        }

        public override bool IsPossible()
        {
            var currentLocation = GameStrategyInfo.CurrentLocation;
            var goalLocation = GameStrategyInfo.UndiscoveredGoalFields[0];
            return currentLocation.Equals(goalLocation);
        }
    }
}