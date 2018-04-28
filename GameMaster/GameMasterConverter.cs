using Common.Interfaces;
using Messaging;

namespace GameMaster
{
    public class GameMasterConverter : MessageConverterBase
    {
        public override IMessage ConvertStringToMessage(string message)
        {
            return MessageSerializer.Deserialize<Message>(message);
        }

    }
}
