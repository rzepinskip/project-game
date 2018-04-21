namespace Common.Interfaces
{
    public interface IMessage
    {
        IMessage Process(IGameMaster gameMaster);
        bool Process(IPlayer player);
        void Process(ICommunicationServer cs, int id);
    }
}