using System;
using System.Collections.Generic;
using System.Text;
using Common.Interfaces;
using Messaging;
using Messaging.Responses;
using Messaging.Serialization;

namespace Player
{
    public class PlayerConverter : IMessageConverter
    {
        private readonly MessageSerializer _messageSerializer;

        public PlayerConverter()
        {
            _messageSerializer = MessageSerializer.Instance;
        }
        
        public string ConvertMessageToString(IMessage message)
        {
            return _messageSerializer.Serialize(message as Message);
        }

        public IMessage ConvertStringToMessage(string message)
        {
            return _messageSerializer.Deserialize<Response>(message);
        }
    }
}
