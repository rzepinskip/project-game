using Common;

namespace GameMaster
{
    public class GameMasterBoard : BoardBase, IGameMasterBoard
    {
        public GameMasterBoard(int boardWidth, int taskAreaSize, int goalAreaSize) : base(boardWidth, taskAreaSize, goalAreaSize)
        {
        }

        public IMessage Discover(string playerGuid)
        {
            throw new System.NotImplementedException();
        }

        public IMessage Move(string playerGuid, Direction direction)
        {
            throw new System.NotImplementedException();
        }

        public IMessage PickUpPiece(string playerGuid)
        {
            throw new System.NotImplementedException();
        }

        public IMessage PlacePiece(string playerGuid)
        {
            throw new System.NotImplementedException();
        }

        public IMessage TestPiece(string playerGuid)
        {
            throw new System.NotImplementedException();
        }
    }
}
