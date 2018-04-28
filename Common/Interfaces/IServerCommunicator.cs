namespace Common.Interfaces
{
    public interface IServerCommunicator
    {
        void Send(IMessage message, int id);
        void StartListening();
    }
}