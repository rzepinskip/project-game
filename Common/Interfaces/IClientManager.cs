namespace Common.Interfaces
{
    public interface IClientManager
    {
        void MarkClientAsPlayer(int id);
        void MarkClientAsGameMaster(int id);
    }
}
