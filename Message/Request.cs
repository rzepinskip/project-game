using System.Xml.Serialization;
using Common;

namespace Message
{
    public abstract class Request : IMessage
    {
        [XmlAttribute] public string PlayerGuid { get; set; }

        [XmlAttribute] public int GameId { get; set; }

        public abstract IMessage Process(IGameMaster gameMaster);
        public abstract void Process(IPlayer player);
    }
}