using System.Xml.Serialization;
using Common;

namespace Message
{
    //[Xml("PlayerMessage")]
    public abstract class Response : IMessage
    {
        protected Response(int playerId)
        {
            PlayerId = playerId;
        }

        [XmlAttribute] public int PlayerId { get; set; }

        public abstract IMessage Process(IGameMaster gameMaster);
        public abstract void Process(IPlayer player);
    }
}