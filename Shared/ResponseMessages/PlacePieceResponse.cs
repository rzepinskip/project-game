﻿using System;
using System.Collections.Generic;
using System.Text;
using Shared.BoardObjects;

namespace Shared.ResponseMessages
{
    public class PlacePieceResponse : ResponseMessage
    {
        public GoalField GoalField { get; set; }
        public override void Update(Board board)
        {
            switch (GoalField.Type)
            {
                case CommonResources.GoalFieldType.NonGoal:
                    //Nothing
                    break;

                case CommonResources.GoalFieldType.Goal:
                    var playerInfo = board.Players[this.PlayerId];
                    playerInfo.Piece = null;
                    var currentGoalField = board.Content[GoalField.X, GoalField.Y] as GoalField;
                    currentGoalField.Type = CommonResources.GoalFieldType.NonGoal;
                    break;

                default:
                    break;
            }
        }
    }
}