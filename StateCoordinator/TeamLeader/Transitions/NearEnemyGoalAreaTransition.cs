using System.Collections.Generic;
using Common.Interfaces;
using PlayerStateCoordinator.Info;
using PlayerStateCoordinator.States;
using PlayerStateCoordinator.TeamLeader.States;
using PlayerStateCoordinator.Transitions;

namespace PlayerStateCoordinator.TeamLeader.Transitions
{
        
    public class NearEnemyGoalAreaTransition : GameStrategyTransition
    {
        public NearEnemyGoalAreaTransition(GameStrategyInfo gameStrategyInfo) : base(gameStrategyInfo)
        {
        }

        public override State NextState => new MovingTowardsEnemyGoalAreaStrategyState(GameStrategyInfo);
        public override IEnumerable<IMessage> Message => new List<IMessage>();
        public override bool IsPossible()
        {
            return !TransitionValidator.IsFarFromEnemyGoalArea(GameStrategyInfo.Team, GameStrategyInfo.Board, GameStrategyInfo.CurrentLocation);
        }
    }

}