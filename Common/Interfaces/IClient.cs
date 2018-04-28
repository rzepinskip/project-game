namespace Common.Interfaces
{
    public interface IClient
    {
        void Send(IMessage message);

        void StartClient();
    }
}
