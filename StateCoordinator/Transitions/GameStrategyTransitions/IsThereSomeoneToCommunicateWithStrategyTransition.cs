using System.Collections.Generic;
using System.Linq;
using Common.Interfaces;
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

        public override State NextState => new InGoalAreaMovingToTaskStrategyState(GameStrategyInfo);

        public override IEnumerable<IMessage> Message
        {
            get
            {
                var withPlayerId = GameStrategyInfo.Board.Players.Values
                    .First(v => v.Id != GameStrategyInfo.PlayerId && v.Team == GameStrategyInfo.Team).Id;

                var request = new AuthorizeKnowledgeExchangeRequest(GameStrategyInfo.PlayerGuid,
                    GameStrategyInfo.GameId, withPlayerId);
                return new List<IMessage> {request};
            }
        }

        public override bool IsPossible()
        {
            var result = GameStrategyInfo.Board.Players.Count > 2;
            //Console.WriteLine($"IsThereSomeoneToCommunicateWithStrategyCondition result {result}");
            return result;
        }
    }
}