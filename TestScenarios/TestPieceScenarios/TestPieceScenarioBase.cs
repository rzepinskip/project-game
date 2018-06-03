using System;

namespace TestScenarios.TestPieceScenarios
{
    public abstract class TestPieceScenarioBase : ScenarioBase
    {
        protected TestPieceScenarioBase(string scenarioName) : base("TestPieceScenarios", scenarioName, 1,
            new Guid("5c6b5263-c614-4608-9136-fcaff688c6ba"))
        {
        }
    }
}