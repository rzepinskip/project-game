using Common.Interfaces;
using Messaging.Serialization;

namespace Messaging
{
    public abstract class Message : IMessage
    {
        public abstract IMessage Process(IGameMaster gameMaster);
        public abstract void Process(IPlayer player);
        public abstract void Process(ICommunicationServer cs, int id);

        public string SerializeToXml()
        {
            return MessageSerializer.Instance.Serialize(this);
        }

        public abstract string ToLog();
    }
}