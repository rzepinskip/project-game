using System;
using System.Collections.Generic;
using System.IO;
using BoardGenerators.Loaders;
using Common.Interfaces;
using GameMaster;
using Player;

namespace TestScenarios
{
    public abstract class ScenarioBase
    {
        private const string GMInitialSufix = "-GM-Initial.xml";
        private const string GMUpdatedSufix = "-GM-Updated.xml";
        private const string PlayerInitialSufix = "-Player-Initial.xml";
        private const string PlayerUpdatedSufix = "-Player-Updated.xml";

        protected ScenarioBase(string scenarioCategory, string scenarioName)
        {
            ScenarioFileBase = Path.Combine(scenarioCategory, scenarioName, scenarioName);
            PlayerGuidToId = new Dictionary<string, int> {{PlayerGuid, PlayerId}};

            LoadBoards();
        }

        public Dictionary<string, int> PlayerGuidToId { get; }
        public int PlayerId { get; } = 1;
        public string PlayerGuid { get; } = Guid.NewGuid().ToString();

        public string ScenarioFileBase { get; }

        public PlayerBoard InitialPlayerBoard { get; protected set; }
        public GameMasterBoard InitialGameMasterBoard { get; protected set; }
        public IRequest InitialRequest { get; protected set; }

        public GameMasterBoard UpdatedGameMasterBoard { get; protected set; } // Validate&Response assert
        public IResponse Response { get; protected set; } // Response assert, UpdatePlayer input
        public PlayerBoard UpdatedPlayerBoard { get; protected set; } // UpdatePlayer output

        private void LoadBoards()
        {
            InitialGameMasterBoard =
                new XmlLoader<GameMasterBoard>().LoadConfigurationFromFile(ScenarioFileBase + GMInitialSufix);
            UpdatedGameMasterBoard =
                new XmlLoader<GameMasterBoard>().LoadConfigurationFromFile(ScenarioFileBase + GMUpdatedSufix);

            InitialPlayerBoard =
                new XmlLoader<PlayerBoard>().LoadConfigurationFromFile(ScenarioFileBase + PlayerInitialSufix);
            UpdatedPlayerBoard =
                new XmlLoader<PlayerBoard>().LoadConfigurationFromFile(ScenarioFileBase + PlayerUpdatedSufix);
        }
    }
}