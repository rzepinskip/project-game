using System.Xml.Serialization;
using Common;

namespace Message
{

    //[Xml("PlayerMessage")]
    public abstract class Response : IMessage
    {
        [XmlAttribute]
        public int PlayerId { get; set; }

        protected Response(int playerId)
        {
            PlayerId = playerId;
        }

        public abstract IMessage Process(IGameMaster gameMaster);
        public abstract void Process(IPlayer player);
    }
}
