using Common.Interfaces;

namespace TestScenarios
{
    public abstract class ScenarioBase
    {
        protected ScenarioBase(Game)
        {
            InitialPlayerBoard = initialPlayerBoard;
            InitGameMasterBoard = initGameMasterBoard;
            InitialRequest = initialRequest;
            UpdatedGameMasterBoard = updatedGameMasterBoard;
            Response = response;
            UpdatedPlayerBoard = updatedPlayerBoard;
        }

        public abstract IPlayerBoard InitialPlayerBoard { get; set; }
        public abstract IGameMasterBoard InitGameMasterBoard { get; set; }
        public abstract IRequest InitialRequest { get; set; }

        public abstract IGameMasterBoard UpdatedGameMasterBoard { get; set; } // Validate&Response assert
        public abstract IResponse Response { get; set; } // Response assert, UpdatePlayer input
        public abstract IPlayerBoard UpdatedPlayerBoard { get; set; } // UpdatePlayer output

        protected IBoard LoadGameMasterBoard()
        {

        }

        protected IBoard LoadPlayerBoard()
        {

        }
    }
}