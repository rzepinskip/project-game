namespace Common.Interfaces
{
    public interface IMessageConverter
    {
        string ConvertMessageToString(IMessage message);
        IMessage ConvertStringToMessage(string message);
    }
}
