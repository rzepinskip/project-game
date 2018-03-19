using Shared.BoardObjects;
using System;
using System.Collections.Generic;
using System.Text;
using static Player.Strategy.PlayerStrategy;
using Shared;

namespace Player.Strategy.StateTransition.Factory
{
    public class StateTranstitionFactory
    {
        private Board board;
        private int playerId;
        private CommonResources.TeamColour teamColour;
        private List<GoalField> undiscoveredGoalFields;
        public StateTranstitionFactory(Board board, int playerId, CommonResources.TeamColour teamColour, List<GoalField> undiscoveredGoalFields)
        {
            this.board = board;
            this.playerId = playerId;
            this.teamColour = teamColour;
            this.undiscoveredGoalFields = undiscoveredGoalFields;
        }

        public BaseTransition GetNextTranstition(PlayerState state, Location location)
        {
            switch (state)
            {
                case PlayerState.InitState:
                    return new InitTransition(location, teamColour, playerId, board);

                case PlayerState.MoveToPiece:
                    return new MoveToPieceTranstition(location, teamColour, playerId, board);

                case PlayerState.RandomWalk:
                    return new RandomWalkTransition(location, teamColour, playerId, board);

                case PlayerState.Discover:
                    return new DiscoverTransition(location, teamColour, playerId, board);

                case PlayerState.Pick:
                    return new PickTransition(location, teamColour, playerId, board);

                case PlayerState.MoveToGoalArea:
                    return new MoveToGoalTransition(undiscoveredGoalFields, location, teamColour, playerId, board);

                case PlayerState.MoveToUndiscoveredGoal:
                    return new MoveToUndiscoveredGoalTransition(undiscoveredGoalFields, location, teamColour, playerId, board);

                case PlayerState.Test:
                    return new TestTransition(location, teamColour, playerId, board);

                case PlayerState.InGoalMovingToTask:
                    return new InGoalMovingToTaskTransition(location, teamColour, playerId, board);

                default:
                    throw new Exception("Factory exception");
            }
        }
    }
}
