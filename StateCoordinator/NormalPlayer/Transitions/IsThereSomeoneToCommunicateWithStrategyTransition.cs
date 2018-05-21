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
    public class IsThereSomeoneToCommunicateWithStrategyTransition : NormalPlayerStrategyTransition
    {
        public IsThereSomeoneToCommunicateWithStrategyTransition(NormalPlayerStrategyInfo normalPlayerStrategyInfo) :
            base(
                normalPlayerStrategyInfo)
        {
        }

        public override State NextState => new InitialMoveAfterPlaceStrategyState(NormalPlayerStrategyInfo);

        public override IEnumerable<IMessage> Message
        {
            get
            {
                var withPlayerId = NormalPlayerStrategyInfo.Board.Players.Values
                    .First(v => v.Id != NormalPlayerStrategyInfo.PlayerId && v.Team == NormalPlayerStrategyInfo.Team)
                    .Id;
                //Console.WriteLine($"Exchanging data with {withPlayerId}");

                var knowledgeExchangeRequest = new AuthorizeKnowledgeExchangeRequest(
                    NormalPlayerStrategyInfo.PlayerGuid,
                    NormalPlayerStrategyInfo.GameId, withPlayerId);
                var dataMessage = DataMessage.FromBoardData(
                    NormalPlayerStrategyInfo.Board.ToBoardData(NormalPlayerStrategyInfo.PlayerId, withPlayerId), false,
                    NormalPlayerStrategyInfo.PlayerGuid);
                return new List<IMessage> {knowledgeExchangeRequest, dataMessage};
            }
        }

        public override bool IsPossible()
        {
            var result = NormalPlayerStrategyInfo.Board.Players.Count > 2 &&
                         NormalPlayerStrategyInfo.IsTimeForExchange();

            return result;
        }
    }
}