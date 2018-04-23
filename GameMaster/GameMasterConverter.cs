using System;
using System.Collections.Generic;
using System.Text;
using Common.Interfaces;
using Messaging;
using Messaging.Requests;
using Messaging.Serialization;

namespace GameMaster
{
    public class GameMasterConverter : MessageConverterBase
    {
        public GameMasterConverter():base()
        {
        }
        public override IMessage ConvertStringToMessage(string message)
        {
            return MessageSerializer.Deserialize<Message>(message);
        }

    }
}
