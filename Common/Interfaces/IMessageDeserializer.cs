using Common.Interfaces;

namespace Communication
{
    public interface IMessageDeserializer
    {
        IMessage Deserialize(string message);
    }
}