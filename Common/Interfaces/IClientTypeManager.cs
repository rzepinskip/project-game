namespace Common.Interfaces
{
    public interface IClientTypeManager
    {
        void MarkClientAsPlayer(int id);
        void MarkClientAsGameMaster(int id);
    }
}