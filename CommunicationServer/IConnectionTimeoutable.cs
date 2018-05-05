namespace CommunicationServer
{
    public interface IConnectionTimeoutable
    {
        void HandleConnectionTimeout(int socketId);
    }
}