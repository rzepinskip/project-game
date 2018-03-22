﻿using System.Xml.Serialization;
using Common.BoardObjects;
using Common.Interfaces;
using Messaging.ActionHelpers;
using Messaging.Responses;

namespace Common.GameMessages.PieceActions
{
    [XmlRoot(Namespace = "https://se2.mini.pw.edu.pl/17-results/")]
    public class PlacePieceRequest : Request
    {
        public override Response Execute(IBoard board)
        {
            ///TODO: different action on TaskField
            var player = board.Players[PlayerId];
            var piece = player.Piece;

            player.Piece = null;

            if (piece.Type == PieceType.Sham)
                return new PlacePieceResponse();

            var playerGoalField = board[player.Location];
            var goalField = playerGoalField as GoalField;


            ///TODO: GameMaster counter
            if (goalField != null) goalField.Type = GoalFieldType.NonGoal;

            var response = new PlacePieceResponse
            {
                PlayerId = PlayerId,
                GoalField = playerGoalField as GoalField
            };

            return response;
        }

        public override ActionLog ToLog(int playerId, PlayerInfo playerInfo)
        {
            return new ActionLog(playerId, GameId, PlayerGuid, playerInfo, ActionType.Place);
        }

        public override double GetDelay(ActionCosts actionCosts)
        {
            return actionCosts.PlacingDelay;
        }
    }
}