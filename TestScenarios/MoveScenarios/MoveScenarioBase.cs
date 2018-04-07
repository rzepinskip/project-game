using System.IO;
using Common;
using Common.Interfaces;
using Messaging.Requests;

namespace TestScenarios.MoveScenarios
{
    public abstract class MoveScenarioBase : ScenarioBase
    {
        protected MoveScenarioBase(string scenarioName) : base(Path.Combine("MoveScenarios","Resources", scenarioName + ".xml"))
        {
        }
    }
}
