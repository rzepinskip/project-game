using System;
using System.Collections.Generic;
using System.Text;
using Common;
using Common.Interfaces;
using Messaging.Requests;
using Player.Strategy.StateInfo;
using Player.Strategy.States;
using Player.Strategy.States.StrategyStates;

namespace Player.Strategy.Conditions.StrategyConditions
{
    public class HasShamCondition : StrategyCondition
    {
        public HasShamCondition(StrategyInfo strategyInfo) : base(strategyInfo) { }
        public override bool CheckCondition()
        {
            var currentLocation = StrategyInfo.FromLocation;
            var goalLocation = StrategyInfo.UndiscoveredGoalFields[0];
            var playerInfo = StrategyInfo.Board.Players[StrategyInfo.PlayerId];
            var piece = playerInfo.Piece;
            var result = false;
            if (piece != null && !currentLocation.Equals(goalLocation))
                if (piece.Type == PieceType.Sham)
                    result = true;
            return result;
        }

        public override BaseState GetNextState(BaseState fromStrategyState)
        {
            return new DestroyPieceStrategyState(StrategyInfo);
        }

        public override IMessage GetNextMessage(BaseState fromStrategyState)
        {
            return new DestroyPieceRequest(StrategyInfo.PlayerGuid, StrategyInfo.GameId);
        }

        public override bool ReturnsMessage(BaseState fromStrategyState)
        {
            return true;
        }
    }
}
