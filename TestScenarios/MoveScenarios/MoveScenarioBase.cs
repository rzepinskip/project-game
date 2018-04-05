using Common;
using Common.Interfaces;
using Messaging.Requests;

namespace TestScenarios.MoveScenarios
{
    public abstract class MoveScenarioBase : ScenarioBase
    {

        public override IRequest InitialRequest { get; protected set; }

        protected MoveScenarioBase(string scenarioFilePath) : base(scenarioFilePath)
        {
        }
    }
}
