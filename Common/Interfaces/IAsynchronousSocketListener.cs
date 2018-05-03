namespace Common.Interfaces
{
    public interface IAsynchronousSocketListener
    {
        void Send(IMessage message, int id);
        void StartListening();
    }
}