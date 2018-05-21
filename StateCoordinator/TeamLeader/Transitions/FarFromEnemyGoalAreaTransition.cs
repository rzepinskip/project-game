using System.Collections.Generic;
using Common.Interfaces;
using Messaging.Requests;
using PlayerStateCoordinator.Common;
using PlayerStateCoordinator.Common.States;
using PlayerStateCoordinator.Common.Transitions;
using PlayerStateCoordinator.TeamLeader.States;

namespace PlayerStateCoordinator.TeamLeader.Transitions
{
    public class FarFromEnemyGoalAreaTransition : GameStrategyTransition
    {
        public FarFromEnemyGoalAreaTransition(GameStrategyInfo gameStrategyInfo) : base(gameStrategyInfo)
        {
        }

        public override State NextState => new MovingTowardsEnemyGoalAreaStrategyState(GameStrategyInfo);

        public override IEnumerable<IMessage> Message
        {
            get
            {
                var direction = GameStrategyInfo.CurrentLocation.DirectionToTask(GameStrategyInfo.Team);
                GameStrategyInfo.TargetLocation =
                    GameStrategyInfo.CurrentLocation.GetNewLocation(direction);

                return new List<IMessage>
                {
                    new MoveRequest(GameStrategyInfo.PlayerGuid,
                    GameStrategyInfo.GameId,
                    direction)
                };
            }
        }
    

        public override bool IsPossible()
        {
            return TransitionValidator.IsFarFromEnemyGoalArea(GameStrategyInfo.Team, GameStrategyInfo.Board,
                GameStrategyInfo.CurrentLocation);
        }
    }
}