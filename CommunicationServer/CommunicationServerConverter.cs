using System;
using System.Collections.Generic;
using System.Text;
using Common.Interfaces;
using Messaging;
using Messaging.Serialization;

namespace CommunicationServer
{
    public class CommunicationServerConverter : MessageConverterBase
    {

        public CommunicationServerConverter():base()
        {
        }

        public override IMessage ConvertStringToMessage(string message)
        {
            return MessageSerializer.Deserialize<Message>(message);
        }
    }
}
