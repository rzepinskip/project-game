using System;
using System.Collections.Generic;
using Common.Interfaces;
using Messaging;
using Messaging.KnowledgeExchangeMessages;
using PlayerStateCoordinator.Common.Transitions;

namespace PlayerStateCoordinator.Common.States
{
    public abstract class GamePlayStrategyState : State
    {
        protected readonly GameStrategyInfo GameStrategyInfo;

        protected GamePlayStrategyState(StateTransitionType transitionType,
            GameStrategyInfo gameStrategyInfo) : base(transitionType, gameStrategyInfo)
        {
            GameStrategyInfo = gameStrategyInfo;
        }
        protected virtual bool IsExchangeWantedWithPlayer(int initiatorId)
        {
            return GameStrategyInfo.Board.Players[initiatorId].Team == GameStrategyInfo.Team;
        }

        protected override Transition HandleRequestMessage(IRequestMessage requestMessage)
        {
            throw new InvalidOperationException($"Not expecting processing incoming request while playing: {requestMessage.GetType().Name}");
        }

        protected override Transition HandleResponseMessage(IResponseMessage responseMessage)
        {
            if (responseMessage is KnowledgeExchangeRequestMessage knowledgeExchangeRequest)
            {
                var initiatorId = knowledgeExchangeRequest.SenderPlayerId;
                //Console.WriteLine($"Player #{initiatorId} requested communication in state {this}");
                IMessage knowledgeExchangeResponse =
                    new RejectKnowledgeExchangeMessage(GameStrategyInfo.PlayerId, initiatorId, GameStrategyInfo.PlayerGuid);

                if (IsExchangeWantedWithPlayer(initiatorId))
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