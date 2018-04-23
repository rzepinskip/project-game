using System;

namespace TestScenarios.DiscoverScenarios
{
    public abstract class DiscoverScenarioBase : ScenarioBase
    {
        protected DiscoverScenarioBase(string scenarioName) : base("DiscoverScenarios", scenarioName, 1,
            new Guid("5c6b5263-c614-4608-9136-fcaff688c6ba"))
        {
        }
    }
}