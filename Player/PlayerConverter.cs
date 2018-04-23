using System;
using System.Collections.Generic;
using System.Text;
using Common.Interfaces;
using Messaging;
using Messaging.Responses;
using Messaging.Serialization;

namespace Player
{
    public class PlayerConverter : MessageConverterBase
    {

        public PlayerConverter() : base()
        {
        }
        

        public override IMessage ConvertStringToMessage(string message)
        {
            return MessageSerializer.Deserialize<Response>(message);
        }
    }
}
