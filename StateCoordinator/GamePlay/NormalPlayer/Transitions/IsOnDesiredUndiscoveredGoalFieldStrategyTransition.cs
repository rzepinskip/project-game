using System.Collections.Generic;
using Common.Interfaces;
using Messaging.Requests;
using PlayerStateCoordinator.Common.States;
using PlayerStateCoordinator.GamePlay.NormalPlayer.States;

namespace PlayerStateCoordinator.GamePlay.NormalPlayer.Transitions
{
    public class IsOnDesiredUndiscoveredGoalFieldStrategyTransition : NormalPlayerStrategyTransition
    {
        public IsOnDesiredUndiscoveredGoalFieldStrategyTransition(NormalPlayerStrategyInfo normalPlayerStrategyInfo) :
            base(
                normalPlayerStrategyInfo)
        {
        }

        public override State NextState => new ReportGoalFieldsStrategyState(NormalPlayerStrategyInfo);

        public override IEnumerable<IMessage> Message
        {
            get
            {
                NormalPlayerStrategyInfo.UndiscoveredGoalFields.RemoveAt(0);
                return new List<IMessage>
                {
                    new PlacePieceRequest(NormalPlayerStrategyInfo.PlayerGuid, NormalPlayerStrategyInfo.GameId)
                };
            }
        }

        public override bool IsPossible()
        {
            var currentLocation = NormalPlayerStrategyInfo.CurrentLocation;
            var goalLocation = NormalPlayerStrategyInfo.UndiscoveredGoalFields[0];
            return currentLocation.Equals(goalLocation);
        }
    }
}