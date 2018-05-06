namespace Common.Interfaces
{
    public interface IAsynchronousSocketListener : IClientTypeManager
    {
        void Send(IMessage message, int id);
        void StartListening();
    }
}