namespace Common.Interfaces
{
    public interface IMessage
    {
        IMessage Process(IGameMaster gameMaster);
        void Process(IGameMaster gameMaster, int i);
        bool Process(IPlayer player);
        void Process(ICommunicationServer cs, int id);
    }
}