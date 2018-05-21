﻿using System.Collections.Generic;
using Common.Interfaces;
using Messaging.Requests;
using PlayerStateCoordinator.Common;
using PlayerStateCoordinator.Common.States;
using PlayerStateCoordinator.Common.Transitions;
using PlayerStateCoordinator.TeamLeader.States;

namespace PlayerStateCoordinator.TeamLeader.Transitions
{
    public class FarFromEnemyGoalAreaTransition : LeaderStrategyTransition
    {
        private readonly LeaderStrategyInfo _leaderStrategyInfo;
        public FarFromEnemyGoalAreaTransition(LeaderStrategyInfo leaderStrategyInfo) : base(leaderStrategyInfo)
        {
            _leaderStrategyInfo = leaderStrategyInfo;
        }

        public override State NextState => new MovingTowardsEnemyGoalAreaStrategyState(_leaderStrategyInfo);

        public override IEnumerable<IMessage> Message
        {
            get
            {
                var direction = LeaderStrategyInfo.CurrentLocation.DirectionToTask(LeaderStrategyInfo.Team);
                LeaderStrategyInfo.TargetLocation =
                    LeaderStrategyInfo.CurrentLocation.GetNewLocation(direction);

                return new List<IMessage>
                {
                    new MoveRequest(LeaderStrategyInfo.PlayerGuid,
                    LeaderStrategyInfo.GameId,
                    direction)
                };
            }
        }
    

        public override bool IsPossible()
        {
            return TransitionValidator.IsFarFromEnemyGoalArea(LeaderStrategyInfo.Team, LeaderStrategyInfo.Board,
                LeaderStrategyInfo.CurrentLocation);
        }
    }
}