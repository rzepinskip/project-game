using System.Xml.Serialization;
using Common;
using Common.Interfaces;

namespace Message
{
    public class MoveMessage : Request
    {
        [XmlAttribute]
        public Direction Direction { get; set; }

        public override IMessage Process(IGameMaster gameMaster)
        {
            
            if (!gameMaster.IsMovePossible(PlayerGuid, Direction))
            {
                // return deny message
            }

            var data = gameMaster.Board.Move(PlayerGuid, Direction);
            return data;
        }

        public override void Process(IPlayer player)
        {
        }
    }
}