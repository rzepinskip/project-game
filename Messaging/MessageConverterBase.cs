using System;
using System.Collections.Generic;
using System.Text;
using Common.Interfaces;
using Messaging.Responses;
using Messaging.Serialization;

namespace Messaging
{
    public abstract class MessageConverterBase : IMessageConverter
    {
        protected readonly MessageSerializer MessageSerializer;

        protected MessageConverterBase()
        {
            MessageSerializer = MessageSerializer.Instance;
        }
        
        public string ConvertMessageToString(IMessage message)
        {
            return MessageSerializer.Serialize(message as Message);
        }

        public abstract IMessage ConvertStringToMessage(string message);

        public byte[] ConvertMessageToBytes(IMessage message, char etbByte)
        {
            return Encoding.ASCII.GetBytes(ConvertMessageToString(message) + etbByte);
        }
    }
}
