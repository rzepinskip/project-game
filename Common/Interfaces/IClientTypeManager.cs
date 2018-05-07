namespace Common.Interfaces
{
    public interface IClientTypeManager
    {
        void MarkClientAsPlayer(int socketId);
        void MarkClientAsGameMaster(int socketId);
        ClientType GetClientTypeFrom(int socketId);
    }
}