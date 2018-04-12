using Common;
using Common.BoardObjects;
using Messaging;
using Messaging.Requests;

namespace TestScenarios.MoveScenarios.MoveToGoalField
{
    public sealed class MoveToGoalField : MoveScenarioBase
    {
        public MoveToGoalField() : base(nameof(MoveToGoalField))
        {
            InitialRequest = new MoveRequest(PlayerGuid, Direction.Up);
            Response = new ResponseWithData(PlayerId, new Location(0, 0));
        }
    }
}
