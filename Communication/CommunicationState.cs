using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Communication
{
    public class CommunicationState
    {
        public const int BufferSize = 1024;        
        public const char EtbByte = (char) 23;

        public CommunicationState()
        {
            LastMessageReceivedTicks = DateTime.Now.Ticks;
            MessageContentBuilder = new StringBuilder();
        }

        public byte[] Buffer { get; } = new byte[BufferSize];
        private StringBuilder MessageContentBuilder { get; }
        public long LastMessageReceivedTicks { get; set; }

        public (IEnumerable<string> messageList, bool hasEtbByte) SplitMessages(int bytesRead, int id)
        {
            MessageContentBuilder.Append(Encoding.ASCII.GetString(Buffer, 0, bytesRead));
            var content = MessageContentBuilder.ToString();
            Debug.WriteLine($"Socket {id}:\nData : {content}");

            var messages = new string[0];
            var hasEtbByte = content.IndexOf(EtbByte) > -1;

            if (hasEtbByte)
            {
                messages = content.Split(EtbByte);
                var numberOfMessages = messages.Length;
                var wholeMessages = string.IsNullOrEmpty(messages[numberOfMessages - 1]);

                MessageContentBuilder.Clear();

                if (!wholeMessages)
                    MessageContentBuilder.Append(messages[numberOfMessages - 1]);
            }

            return (messages.Take(messages.Length - 1), hasEtbByte);
        }

        public void UpdateLastMessageTicks()
        {
            LastMessageReceivedTicks = DateTime.Now.Ticks;
        }
    }
}