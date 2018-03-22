using Common;
using Common.BoardObjects;
using Common.Interfaces;

namespace Messaging.Responses
{
    public class PlacePieceResponse : Response
    {
        public PlacePieceResponse(int playerId, GoalField goalField = null, bool isGameFinished = false) : base(
            playerId, isGameFinished)
        {
            GoalField = goalField;
        }

        public GoalField GoalField { get; }

        public override void Update(IBoard board)
        {
            var playerInfo = board.Players[PlayerId];
            playerInfo.Piece = null;
            switch (GoalField.Type)
            {
                case GoalFieldType.NonGoal:
                    //Nothing
                    break;

                case GoalFieldType.Goal:
                    var currentGoalField = board[GoalField] as GoalField;
                    currentGoalField.Type = GoalFieldType.NonGoal;
                    break;

                default:
                    break;
            }
        }
    }
}