namespace Common.Interfaces
{
    public interface IResponse : IMessage
    {
        int PlayerId { get; set; }
    }
}