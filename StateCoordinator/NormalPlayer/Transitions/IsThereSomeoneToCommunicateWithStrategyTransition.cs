using System;
using System.Collections.Generic;
using System.Linq;
using Common.Interfaces;
using Messaging;
using Messaging.ActionsMessages;
using PlayerStateCoordinator.Common;
using PlayerStateCoordinator.Common.States;
using PlayerStateCoordinator.Common.Transitions;
using PlayerStateCoordinator.NormalPlayer.States;

namespace PlayerStateCoordinator.NormalPlayer.Transitions
{
    public class IsThereSomeoneToCommunicateWithStrategyTransition : GameStrategyTransition
    {
        public IsThereSomeoneToCommunicateWithStrategyTransition(GameStrategyInfo gameStrategyInfo) : base(
            gameStrategyInfo)
        {
        }

        public override State NextState => new InitialMoveAfterPlaceStrategyState(GameStrategyInfo);

        public override IEnumerable<IMessage> Message
        {
            get
            {
                var withPlayerId = GameStrategyInfo.Board.Players.Values
                    .First(v => v.Id != GameStrategyInfo.PlayerId && v.Team == GameStrategyInfo.Team).Id;
                //Console.WriteLine($"Exchanging data with {withPlayerId}");

                var knowledgeExchangeRequest = new AuthorizeKnowledgeExchangeRequest(GameStrategyInfo.PlayerGuid,
                    GameStrategyInfo.GameId, withPlayerId);
                var dataMessage = DataMessage.FromBoardData(
                    GameStrategyInfo.Board.ToBoardData(GameStrategyInfo.PlayerId, withPlayerId), false,
                    GameStrategyInfo.PlayerGuid);
                return new List<IMessage> {knowledgeExchangeRequest, dataMessage};
            }
        }

        public override bool IsPossible()
        {
            var result = GameStrategyInfo.Board.Players.Count > 2 && GameStrategyInfo.IsTimeForExchange();

            return result;
        }
    }
}