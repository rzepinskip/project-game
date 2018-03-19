using System;
using System.Collections.Generic;
using Shared;
using Shared.BoardObjects;
using static Player.Strategy.PlayerStrategy;

namespace Player.Strategy.StateTransition.Factory
{
    public class StateTranstitionFactory
    {
        private readonly Board board;
        private readonly int playerId;
        private readonly CommonResources.TeamColour teamColour;
        private readonly List<GoalField> undiscoveredGoalFields;

        public StateTranstitionFactory(Board board, int playerId, CommonResources.TeamColour teamColour,
            List<GoalField> undiscoveredGoalFields)
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
                    return new MoveToUndiscoveredGoalTransition(undiscoveredGoalFields, location, teamColour, playerId,
                        board);

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