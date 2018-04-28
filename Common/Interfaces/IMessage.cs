namespace Common.Interfaces
{
    public interface IMessage
    {
        IMessage Process(IGameMaster gameMaster);
        void Process(IPlayer player);
        void Process(ICommunicationServerCommunicator cs, int id);
    }
}