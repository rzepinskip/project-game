using System;
using System.Collections.Generic;
using BoardGenerators.Loaders;
using Common.Interfaces;
using GameMaster;
using Messaging.Requests;
using Player;
using TestScenarios.DeterministicGame;

namespace TestScenarios
{
    public abstract class ScenarioBase
    {
        public Dictionary<string, int> PlayerGuidToId { get; }

        protected int PlayerId { get; set; } = 0;
        protected string PlayerGuid { get; set; } = Guid.NewGuid().ToString();

        public string ScenarioFilePath { get; }

        protected ScenarioBase(string scenarioFilePath)
        {
            ScenarioFilePath = scenarioFilePath;
            PlayerGuidToId = new Dictionary<string, int>();
            PlayerGuidToId.Add(PlayerGuid, PlayerId);

        }

        public abstract PlayerBoard InitialPlayerBoard { get; protected set; }
        public abstract GameMasterBoard InitGameMasterBoard { get; protected set; }
        public abstract IRequest InitialRequest { get; protected set; }

        public abstract GameMasterBoard UpdatedGameMasterBoard { get; protected set; } // Validate&Response assert
        public abstract IResponse Response { get; protected set; } // Response assert, UpdatePlayer input
        public abstract PlayerBoard UpdatedPlayerBoard { get; protected set; } // UpdatePlayer output

        protected GameMasterBoard LoadGameMasterBoard()
        {
            var gameDefinition = new XmlLoader<DeterministicGameDefinition>().LoadConfigurationFromFile(ScenarioFilePath);
            return new DeterministicGameMasterBoardGenerator().InitializeBoard(gameDefinition);
        }

        protected PlayerBoard LoadPlayerBoard()
        {
            var gameDefinition = new XmlLoader<DeterministicGameDefinition>().LoadConfigurationFromFile(ScenarioFilePath);
            return new DeterministicPlayerBoardGenerator().InitializeBoard(gameDefinition, 0);
        }
    }
}