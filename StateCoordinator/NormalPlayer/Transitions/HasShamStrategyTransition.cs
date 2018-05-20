using System.Collections.Generic;
using Common;
using Common.Interfaces;
using Messaging.Requests;
using PlayerStateCoordinator.Common;
using PlayerStateCoordinator.Common.States;
using PlayerStateCoordinator.Common.Transitions;
using PlayerStateCoordinator.Info;
using PlayerStateCoordinator.NormalPlayer.States;

namespace PlayerStateCoordinator.NormalPlayer.Transitions
{
    public class HasShamStrategyTransition : GameStrategyTransition
    {
        public HasShamStrategyTransition(GameStrategyInfo gameStrategyInfo) : base(gameStrategyInfo)
        {
        }

        public override State NextState => new DestroyPieceStrategyState(GameStrategyInfo);

        public override IEnumerable<IMessage> Message => new List<IMessage>
        {
            new DestroyPieceRequest(GameStrategyInfo.PlayerGuid, GameStrategyInfo.GameId)
        };

        public override bool IsPossible()
        {
            var currentLocation = GameStrategyInfo.CurrentLocation;
            var goalLocation = GameStrategyInfo.UndiscoveredGoalFields[0];
            var playerInfo = GameStrategyInfo.Board.Players[GameStrategyInfo.PlayerId];
            var piece = playerInfo.Piece;
            var result = false;
            if (piece != null && !currentLocation.Equals(goalLocation))
                if (piece.Type == PieceType.Sham)
                    result = true;
            return result;
        }
    }
}