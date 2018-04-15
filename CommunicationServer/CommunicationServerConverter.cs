using System;
using System.Collections.Generic;
using System.Text;
using Common.Interfaces;
using Messaging;
using Messaging.Serialization;

namespace CommunicationServer
{
    public class CommunicationServerConverter : IMessageConverter
    {
        private readonly MessageSerializer _messageSerializer;

        public CommunicationServerConverter()
        {
            _messageSerializer = MessageSerializer.Instance;
        }
        public string ConvertMessageToString(IMessage message)
        {
            return _messageSerializer.Serialize(message as Message);
        }

        public IMessage ConvertStringToMessage(string message)
        {
            return _messageSerializer.Deserialize<Message>(message);
        }
    }
}
