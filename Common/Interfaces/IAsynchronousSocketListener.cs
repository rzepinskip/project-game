namespace Common.Interfaces
{
    public interface IAsynchronousSocketListener: IClientManager
    {
        void Send(IMessage message, int id);
        void StartListening();
    }
}