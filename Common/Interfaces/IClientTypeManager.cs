namespace Common.Interfaces
{
    public interface IClientTypeManager
    {
        void MarkClientAsPlayer(int connectionId);
        void MarkClientAsGameMaster(int connectionId);
    }
}