using Common;
using Common.BoardObjects;
using Common.Interfaces;
using GameMaster;
using Messaging;
using Messaging.Requests;
using Player;

namespace TestScenarios.MoveScenarios
{
    public sealed class MoveToGoalField : MoveScenarioBase
    {
        public override PlayerBoard InitialPlayerBoard { get; protected set; }
        public override GameMasterBoard InitialGameMasterBoard { get; protected set; }
        public override IRequest InitialRequest { get; protected set; }

        public override GameMasterBoard UpdatedGameMasterBoard { get; protected set; }
        public override IResponse Response { get; protected set; }
        public override PlayerBoard UpdatedPlayerBoard { get; protected set; }

        public MoveToGoalField() : base(nameof(MoveToGoalField))
        {
            InitialRequest = new MoveRequest(PlayerGuid, Direction.Up);

            var finishLocation = new Location(0, 1);
            var startLocation = new Location(0, 0);
            Response = new ResponseWithData(PlayerId, finishLocation);

            InitialPlayerBoard = LoadPlayerBoard();
            UpdatedPlayerBoard = LoadPlayerBoard();

            InitialGameMasterBoard = LoadGameMasterBoard();
            UpdatedGameMasterBoard = LoadGameMasterBoard();


            UpdatedGameMasterBoard[startLocation].PlayerId = null;
            UpdatedGameMasterBoard[finishLocation].PlayerId = PlayerId;
            UpdatedGameMasterBoard.Players[PlayerId].Location = finishLocation;

            UpdatedPlayerBoard.Players[PlayerId].Location = finishLocation;
            UpdatedPlayerBoard[startLocation].PlayerId = null;
            UpdatedPlayerBoard[finishLocation].PlayerId = PlayerId;
        }
    }
}
