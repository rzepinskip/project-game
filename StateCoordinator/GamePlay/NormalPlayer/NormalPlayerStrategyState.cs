using System;
using System.Linq;
using Common;
using Common.BoardObjects;
using PlayerStateCoordinator.Common;

namespace PlayerStateCoordinator.GamePlay.NormalPlayer
{
    public class NormalPlayerStrategyState : GamePlayStrategyState
    {
        public NormalPlayerStrategyState(StateTransitionType transitionType,
            NormalPlayerStrategyInfo playerStrategyInfo) : base(transitionType, playerStrategyInfo)
        {
        }

        protected override void HandleSuccessfulKnowledgeExchange()
        {
            var currentInfo = PlayerStrategyInfo as NormalPlayerStrategyInfo;
            var beforeExchangeCount = currentInfo.UndiscoveredGoalFields.Count;

            foreach (var field in PlayerStrategyInfo.Board.ToEnumerable())
            {
                if (!(field is GoalField goalField) || goalField.Type == GoalFieldType.Unknown)
                    continue;

                var toRemove = currentInfo.UndiscoveredGoalFields.FirstOrDefault(g => g.X == goalField.X && g.Y == goalField.Y);

                if (toRemove != default(GoalField))
                {
                    //Console.WriteLine($"Removed goal: {goalField}");
                    currentInfo.UndiscoveredGoalFields.Remove(toRemove);
                }
            }

            Console.WriteLine($"Removed {beforeExchangeCount - currentInfo.UndiscoveredGoalFields.Count} goalfields already tested by others");
        }
    }
}