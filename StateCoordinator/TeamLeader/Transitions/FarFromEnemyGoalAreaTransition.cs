using System.Collections.Generic;
using Common;
using Common.BoardObjects;
using Common.Interfaces;
using Messaging.Requests;
using PlayerStateCoordinator.Info;
using PlayerStateCoordinator.States;
using PlayerStateCoordinator.TeamLeader.States;
using PlayerStateCoordinator.Transitions;

namespace PlayerStateCoordinator.TeamLeader.Transitions
{
    public class FarFromEnemyGoalAreaTransition : GameStrategyTransition
    {
        public FarFromEnemyGoalAreaTransition(GameStrategyInfo gameStrategyInfo) : base(gameStrategyInfo)
        {
        }

        public override State NextState => new MovingTowardsEnemyGoalAreaStrategyState(GameStrategyInfo);

        public override IEnumerable<IMessage> Message => new List<IMessage>
        {
            new MoveRequest(GameStrategyInfo.PlayerGuid, GameStrategyInfo.GameId,
                GameStrategyInfo.CurrentLocation.DirectionToTask(GameStrategyInfo.Team))
        };

        public override bool IsPossible()
        {
            return TransitionValidator.IsFarFromEnemyGoalArea(GameStrategyInfo.Team, GameStrategyInfo.Board, GameStrategyInfo.CurrentLocation);
        }
    }
}