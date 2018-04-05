using System;
using BoardGenerators.Loaders;
using Common.Interfaces;
using TestScenarios.DeterministicGame;

namespace TestScenarios
{
    public abstract class ScenarioBase
    {
        protected int PlayerId { get; set; } = 0;
        protected string PlayerGuid { get; set; } = Guid.NewGuid().ToString();

        public string ScenarioFilePath { get; }

        protected ScenarioBase(string scenarioName)
        {
             ScenarioFilePath = scenarioName + ".xml";
        }

        public abstract IPlayerBoard InitialPlayerBoard { get; protected set; }
        public abstract IGameMasterBoard InitGameMasterBoard { get; protected set; }
        public abstract IRequest InitialRequest { get; protected set; }

        public abstract IGameMasterBoard UpdatedGameMasterBoard { get; protected set; } // Validate&Response assert
        public abstract IResponse Response { get; protected set; } // Response assert, UpdatePlayer input
        public abstract IPlayerBoard UpdatedPlayerBoard { get; protected set; } // UpdatePlayer output

        protected IGameMasterBoard LoadGameMasterBoard()
        {
            var gameDefinition = new XmlLoader<DeterministicGameDefinition>().LoadConfigurationFromFile(ScenarioFilePath);
            return new DeterministicGameMasterBoardGenerator().InitializeBoard(gameDefinition);
        }

        protected IPlayerBoard LoadPlayerBoard()
        {
            var gameDefinition = new XmlLoader<DeterministicGameDefinition>().LoadConfigurationFromFile(ScenarioFilePath);
            return new DeterministicPlayerBoardGenerator().InitializeBoard(gameDefinition, 0);
        }
    }
}