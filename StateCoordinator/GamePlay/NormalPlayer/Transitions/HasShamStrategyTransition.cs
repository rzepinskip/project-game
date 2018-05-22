using System.Collections.Generic;
using Common;
using Common.Interfaces;
using Messaging.Requests;
using PlayerStateCoordinator.Common.States;
using PlayerStateCoordinator.GamePlay.NormalPlayer.States;

namespace PlayerStateCoordinator.GamePlay.NormalPlayer.Transitions
{
    public class HasShamStrategyTransition : NormalPlayerStrategyTransition
    {
        public HasShamStrategyTransition(NormalPlayerStrategyInfo normalPlayerStrategyInfo) : base(
            normalPlayerStrategyInfo)
        {
        }

        public override State NextState => new DestroyPieceStrategyState(NormalPlayerStrategyInfo);

        public override IEnumerable<IMessage> Message => new List<IMessage>
        {
            new DestroyPieceRequest(NormalPlayerStrategyInfo.PlayerGuid, NormalPlayerStrategyInfo.GameId)
        };

        public override bool IsPossible()
        {
            var currentLocation = NormalPlayerStrategyInfo.CurrentLocation;
            var goalLocation = NormalPlayerStrategyInfo.UndiscoveredGoalFields[0];
            var playerInfo = NormalPlayerStrategyInfo.Board.Players[NormalPlayerStrategyInfo.PlayerId];
            var piece = playerInfo.Piece;
            var result = false;
            if (piece != null && !currentLocation.Equals(goalLocation))
                if (piece.Type == PieceType.Sham)
                    result = true;
            return result;
        }
    }
}