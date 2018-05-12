using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Communication.TcpConnection
{
    public class CommunicationState
    {
        private readonly StringBuilder _messageContentBuilder;

        public CommunicationState()
        {
            LastReceivedMessageTicks = DateTime.Now.Ticks;
            _messageContentBuilder = new StringBuilder();
        }

        public long LastReceivedMessageTicks { get; private set; }
        public byte[] Buffer { get; } = new byte[Constants.BufferSize];

        public (IEnumerable<string> messageList, bool hasEtbByte) SplitMessages(int bytesRead, int connectionId)
        {
            _messageContentBuilder.Append(Encoding.ASCII.GetString(Buffer, 0, bytesRead));
            var content = _messageContentBuilder.ToString();
            Debug.WriteLine($"Socket {connectionId}:\nData : {content}");

            var messages = new string[0];
            var hasEtbByte = content.IndexOf(Constants.EtbByte) > -1;

            if (hasEtbByte)
            {
                messages = content.Split(Constants.EtbByte);
                var numberOfMessages = messages.Length;
                var wholeMessages = string.IsNullOrEmpty(messages[numberOfMessages - 1]);

                _messageContentBuilder.Clear();

                if (!wholeMessages)
                    _messageContentBuilder.Append(messages[numberOfMessages - 1]);
            }

            return (messages.Take(messages.Length - 1), hasEtbByte);
        }

        public void UpdateLastReceivedMessageTicks()
        {
            LastReceivedMessageTicks = DateTime.Now.Ticks;
        }
    }
}