namespace Common.Interfaces
{
    public interface ICommunicationServer : ICommunicationRouter, IClientTypeManager
    {
        void Send(IMessage message, int socketId);
    }
}