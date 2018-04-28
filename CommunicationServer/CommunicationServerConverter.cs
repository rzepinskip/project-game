using Common.Interfaces;
using Messaging;

namespace CommunicationServer
{
    public class CommunicationServerConverter : MessageConverterBase
    {
        public override IMessage ConvertStringToMessage(string message)
        {
            return MessageSerializer.Deserialize<Message>(message);
        }
    }
}