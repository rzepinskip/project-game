using Shared.BoardObjects;

namespace Shared.ResponseMessages
{
    public class PlacePieceResponse : ResponseMessage
    {
        public GoalField GoalField { get; set; }

        public override void Update(Board board)
        {
            var playerInfo = board.Players[PlayerId];
            playerInfo.Piece = null;
            switch (GoalField.Type)
            {
                case CommonResources.GoalFieldType.NonGoal:
                    //Nothing
                    break;

                case CommonResources.GoalFieldType.Goal:
                    var currentGoalField = board.Content[GoalField.X, GoalField.Y] as GoalField;
                    currentGoalField.Type = CommonResources.GoalFieldType.NonGoal;
                    break;

                default:
                    break;
            }
        }
    }
}