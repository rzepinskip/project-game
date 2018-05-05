using System;

namespace TestScenarios.PickUpScenarios
{
    public abstract class PickUpScenarioBase : ScenarioBase
    {
        protected PickUpScenarioBase(string scenarioName) : base("PickUpScenarios", scenarioName, 1,
            new Guid("5c6b5263-c614-4608-9136-fcaff688c6ba"))
        {
        }
    }
}