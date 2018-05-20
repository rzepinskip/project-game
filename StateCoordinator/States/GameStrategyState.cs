using System;
using System.Collections.Generic;
using Common.Interfaces;
using Messaging;
using Messaging.KnowledgeExchangeMessages;
using PlayerStateCoordinator.Info;
using PlayerStateCoordinator.Transitions;

namespace PlayerStateCoordinator.States
{
    public class GameStrategyState : State
    {
        protected readonly GameStrategyInfo GameStrategyInfo;

        public GameStrategyState(StateTransitionType transitionType,
            GameStrategyInfo gameStrategyInfo) : base(transitionType, gameStrategyInfo)
        {
            GameStrategyInfo = gameStrategyInfo;
        }

        protected override Transition HandleResponseMessage(IResponseMessage responseMessage)
        {
            if (responseMessage is KnowledgeExchangeRequestMessage knowledgeExchangeRequest)
            {
                var initiatorId = knowledgeExchangeRequest.SenderPlayerId;
                //Console.WriteLine($"Player #{initiatorId} requested communication in state {this}");
                IMessage knowledgeExchangeResponse =
                    new RejectKnowledgeExchangeMessage(GameStrategyInfo.PlayerId, initiatorId, GameStrategyInfo.PlayerGuid);

                if (GameStrategyInfo.Board.Players[initiatorId].Team == GameStrategyInfo.Team)
                    knowledgeExchangeResponse =
                        DataMessage.FromBoardData(
                            GameStrategyInfo.Board.ToBoardData(GameStrategyInfo.PlayerId, initiatorId), false,
                            GameStrategyInfo.PlayerGuid);

                return new LoopbackTransition(this, new List<IMessage> {knowledgeExchangeResponse});
            }

            if (responseMessage is DataMessage dataMessage && dataMessage.GoalFields.Length > 1)
            {
                //Console.WriteLine($"Got some data, doing nothing");

                return new LoopbackTransition(this, new List<IMessage>());
            }

            return base.HandleResponseMessage(responseMessage);
        }

        protected override Transition HandleErrorMessage(IErrorMessage errorMessage)
        {
            Console.WriteLine("Got error messages, proceeding to strategy reset");
            return new ErrorTransition(GameStrategyInfo.PlayerGameStrategyBeginningState);
        }

        protected override Transition HandleNoMatchingTransitionCase()
        {
            throw new StrategyException(this, "No matching transition");
        }
    }
}