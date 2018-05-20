using System;
using System.Collections.Generic;
using System.Linq;
using Common.Interfaces;
using Messaging;
using Messaging.ActionsMessages;
using PlayerStateCoordinator.Info;
using PlayerStateCoordinator.States;
using PlayerStateCoordinator.States.GameStrategyStates;

namespace PlayerStateCoordinator.Transitions.GameStrategyTransitions
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
                Console.WriteLine($"Exchanging data with {withPlayerId}");

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
            var result = GameStrategyInfo.Board.Players.Count > 2;
            return result;
        }
    }
}