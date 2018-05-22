using System;
using System.Collections.Generic;
using Common.Interfaces;
using Messaging;
using Messaging.KnowledgeExchangeMessages;
using PlayerStateCoordinator.Common;
using PlayerStateCoordinator.Common.States;
using PlayerStateCoordinator.Common.Transitions;

namespace PlayerStateCoordinator.GamePlay
{
    public abstract class GamePlayStrategyState : State
    {
        protected readonly GamePlayStrategyInfo PlayerStrategyInfo;
        public bool RestrictedToTaskArea { get; protected set; }

        protected GamePlayStrategyState(StateTransitionType transitionType,
            GamePlayStrategyInfo playerStrategyInfo) : base(transitionType, playerStrategyInfo)
        {
            RestrictedToTaskArea = false;
            PlayerStrategyInfo = playerStrategyInfo;
        }

        protected virtual bool IsExchangeWantedWithPlayer(int initiatorId)
        {
            return PlayerStrategyInfo.Board.Players[initiatorId].Team == PlayerStrategyInfo.Team;
        }

        protected override Transition HandleRequestMessage(IRequestMessage requestMessage)
        {
            throw new InvalidOperationException(
                $"Not expecting processing incoming request while playing: {requestMessage.GetType().Name}");
        }

        protected override Transition HandleResponseMessage(IResponseMessage responseMessage)
        {
            if (responseMessage is KnowledgeExchangeRequestMessage knowledgeExchangeRequest)
            {
                var initiatorId = knowledgeExchangeRequest.SenderPlayerId;
                //Console.WriteLine($"Player #{initiatorId} requested communication in state {this}");
                IMessage knowledgeExchangeResponse =
                    new RejectKnowledgeExchangeMessage(PlayerStrategyInfo.PlayerId, initiatorId,
                        PlayerStrategyInfo.PlayerGuid);

                if (IsExchangeWantedWithPlayer(initiatorId))
                    knowledgeExchangeResponse =
                        DataMessage.FromBoardData(
                            PlayerStrategyInfo.Board.ToBoardData(PlayerStrategyInfo.PlayerId, initiatorId), false,
                            PlayerStrategyInfo.PlayerGuid);

                return new LoopbackTransition(this, new List<IMessage> {knowledgeExchangeResponse});
            }

            if (responseMessage is DataMessage dataMessage && dataMessage.GoalFields.Length > 1)
                return new LoopbackTransition(this, new List<IMessage>());

            return base.HandleResponseMessage(responseMessage);
        }

        protected override Transition HandleErrorMessage(IErrorMessage errorMessage)
        {
            Console.WriteLine("Got error messages, proceeding to strategy reset");
            return new ErrorTransition(PlayerStrategyInfo.PlayerGameStrategyBeginningState);
        }

        protected override Transition HandleNoMatchingTransitionCase()
        {
            throw new StrategyException(this, "No matching transition");
        }
    }
}