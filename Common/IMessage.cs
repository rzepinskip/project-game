namespace Common
{
    public interface IMessage
    {
        IMessage Process(IGameMaster gameMaster);
        void Process(IPlayer player);
    }
}