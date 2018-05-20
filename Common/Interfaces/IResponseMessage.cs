namespace Common.Interfaces
{
    public interface IResponseMessage : IMessage
    {
        int PlayerId { get; set; }
    }
}