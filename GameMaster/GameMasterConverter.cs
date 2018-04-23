using System;
using System.Collections.Generic;
using System.Text;
using Common.Interfaces;
using Messaging;
using Messaging.Requests;
using Messaging.Serialization;

namespace GameMaster
{
    public class GameMasterConverter : IMessageConverter
    {
        private readonly MessageSerializer _messageSerializer;
        public GameMasterConverter()
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
