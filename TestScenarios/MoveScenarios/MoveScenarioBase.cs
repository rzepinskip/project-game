using System;

namespace TestScenarios.MoveScenarios
{
    public abstract class MoveScenarioBase : ScenarioBase
    {
        protected MoveScenarioBase(string scenarioName) : base("MoveScenarios", scenarioName, 1,
            new Guid("5c6b5263-c614-4608-9136-fcaff688c6ba"))
        {
        }
    }
}