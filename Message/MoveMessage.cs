using System;
using System.Xml.Serialization;
using Common;
using Shared;

namespace Message
{
    public class MoveMessage : Request
    {
        [XmlAttribute] public CommonResources.MoveType Direction { get; set; }

        public override IMessage Process(IGameMaster gameMaster)
        {
            var data = gameMaster.Move(PlayerGuid, Direction);
            return data;
        }


        public override void Process(IPlayer player)
        {
            throw new NotImplementedException();
        }
    }
}