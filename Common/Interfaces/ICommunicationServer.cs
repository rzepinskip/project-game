namespace Common.Interfaces
{
    public interface ICommunicationServer : ICommunicationRouter, IAsynchronousSocketListener, IClientTypeManager
    {
    }
}