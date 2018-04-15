using Common;
using Common.BoardObjects;
using Messaging.Requests;
using Messaging.Responses;

namespace TestScenarios.MoveScenarios.MoveToGoalField
{
    public sealed class MoveToGoalField : MoveScenarioBase
    {
        public MoveToGoalField() : base(nameof(MoveToGoalField))
        {
            Response = new ResponseWithData(PlayerId, new Location(0, 0));
            InitialRequest = new MoveRequest(PlayerGuid, Direction.Left);
        }
    }
}
