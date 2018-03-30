using Common;
using Common.Interfaces;
using Messaging.Requests;

namespace TestScenarios.MoveScenarios
{
    public abstract class MoveScenarioBase : ScenarioBase
    {
        protected int PlayerId { get; set; } = 0;

        public override IRequest InitialRequest { get; set; } = new MoveRequest(PlayerId, Direction.Down);
    }
}
