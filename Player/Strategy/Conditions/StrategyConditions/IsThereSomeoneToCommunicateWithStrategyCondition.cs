using System;
using System.Linq;
using Common.Interfaces;
using Messaging.ActionsMessages;
using Player.Strategy.StateInfo;
using Player.Strategy.States;
using Player.Strategy.States.StrategyStates;

namespace Player.Strategy.Conditions.StrategyConditions
{
    internal class IsThereSomeoneToCommunicateWithStrategyCondition : StrategyCondition
    {
        public IsThereSomeoneToCommunicateWithStrategyCondition(StrategyInfo strategyInfo) : base(strategyInfo)
        {
        }

        public override bool CheckCondition()
        {
            var result = StrategyInfo.Board.Players.Count > 2;
            //Console.WriteLine($"IsThereSomeoneToCommunicateWithStrategyCondition result {result}");
            return result;
        }

        public override BaseState GetNextState(BaseState fromStrategyState)
        {
            return new InGoalAreaMovingToTaskStrategyState(StrategyInfo);
        }

        public override IMessage GetNextMessage(BaseState fromStrategyState)
        {
            var withPlayerId = StrategyInfo.Board.Players.Values
                .First(v => v.Id != StrategyInfo.PlayerId && v.Team == StrategyInfo.Team).Id;

            return new AuthorizeKnowledgeExchangeRequest(StrategyInfo.PlayerGuid, StrategyInfo.GameId, withPlayerId);
        }

        public override bool ReturnsMessage(BaseState fromStrategyState)
        {
            return true;
        }
    }
}