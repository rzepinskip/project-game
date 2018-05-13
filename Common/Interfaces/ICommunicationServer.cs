namespace Common.Interfaces
{
    public interface ICommunicationServer : IGamesManager, IClientTypeManager
    {
        void Send(IMessage message, int connectionId);
    }
}