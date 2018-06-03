using System;

namespace TestScenarios.PlaceScenarios
{
    public abstract class PlaceScenarioBase : ScenarioBase
    {
        protected PlaceScenarioBase(string scenarioName) : base("PlaceScenarios", scenarioName, 1,
            new Guid("5c6b5263-c614-4608-9136-fcaff688c6ba"))
        {
        }
    }
}