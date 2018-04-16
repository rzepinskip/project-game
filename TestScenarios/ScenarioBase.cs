using System;
using System.Collections.Generic;
using System.IO;
using Common;
using GameMaster;
using Messaging;
using Messaging.Serialization;
using Player;

namespace TestScenarios
{
    public abstract class ScenarioBase
    {
        private const string TestFileExtension = ".xml";
        private const string DefaultNamespace = "https://se2.mini.pw.edu.pl/17-results/";

        private const string GMInitialSufix = "-GM-Initial" + TestFileExtension;
        private const string GMUpdatedSufix = "-GM-Updated" + TestFileExtension;
        private const string PlayerInitialSufix = "-Player-Initial" + TestFileExtension;
        private const string PlayerUpdatedSufix = "-Player-Updated" + TestFileExtension;
        private const string RequestSufix = "-Request" + TestFileExtension;
        private const string ResponseSufix = "-Response" + TestFileExtension;
        private const string IdentifiersSufix = "-Identifiers" + TestFileExtension;


        protected ScenarioBase(string scenarioCategory, string scenarioName, int playerId, Guid playerGuid)
        {
            ScenarioFileBase = Path.Combine(scenarioCategory, scenarioName, scenarioName);
            PlayerId = playerId;
            PlayerGuid = playerGuid;

            LoadBoards();
            LoadMessages();
            LoadIdentifiers();
        }

        public Dictionary<Guid, int> PlayerGuidToId { get; private set; }
        public int PlayerId { get; }
        public Guid PlayerGuid { get; }

        public string ScenarioFileBase { get; }

        public PlayerBoard InitialPlayerBoard { get; private set; }
        public GameMasterBoard InitialGameMasterBoard { get; private set; }
        public string InitialRequestFileContent { get; private set; }
        public Message InitialRequest { get; private set; }

        public GameMasterBoard UpdatedGameMasterBoard { get; private set; } // Validate&Response assert
        public string ResponseFileContent { get; private set; }
        public Message Response { get; private set; } // Response assert, UpdatePlayer input
        public PlayerBoard UpdatedPlayerBoard { get; private set; } // UpdatePlayer output

        private void LoadBoards()
        {
            var xmlSerializer = new ExtendedXmlSerializer(string.Empty);

            InitialGameMasterBoard =
                xmlSerializer.DeserializeFromXmlFile<GameMasterBoard>(ScenarioFileBase + GMInitialSufix);
            UpdatedGameMasterBoard =
                xmlSerializer.DeserializeFromXmlFile<GameMasterBoard>(ScenarioFileBase + GMUpdatedSufix);

            InitialPlayerBoard =
                xmlSerializer.DeserializeFromXmlFile<PlayerBoard>(ScenarioFileBase + PlayerInitialSufix);
            UpdatedPlayerBoard =
                xmlSerializer.DeserializeFromXmlFile<PlayerBoard>(ScenarioFileBase + PlayerUpdatedSufix);
        }

        private void LoadMessages()
        {
            var messageXmlSerializer = new ExtendedMessageXmlDeserializer(DefaultNamespace);

            InitialRequest = messageXmlSerializer.DeserializeFromFile(ScenarioFileBase + RequestSufix);
            Response = messageXmlSerializer.DeserializeFromFile(ScenarioFileBase + ResponseSufix);

            using (var reader = new StreamReader(ScenarioFileBase + RequestSufix))
            {
                InitialRequestFileContent = reader.ReadToEnd();
            }
            using (var reader = new StreamReader(ScenarioFileBase + ResponseSufix))
            {
                ResponseFileContent = reader.ReadToEnd();
            }
        }

        private void LoadIdentifiers()
        {
            PlayerGuidToId =
                new ExtendedXmlSerializer(string.Empty).DeserializeFromXmlFile<SerializableDictionary<Guid, int>>(
                    ScenarioFileBase + IdentifiersSufix);
        }
    }
}