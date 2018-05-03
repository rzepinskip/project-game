namespace Common.Interfaces
{
    public interface IMessage
    {
        IMessage Process(IGameMaster gameMaster);
        void Process(IPlayer player);
        void Process(ICommunicationServer cs, int id);
        string SerializeToXml();
    }
}