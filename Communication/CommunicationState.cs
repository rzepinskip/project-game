using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Communication
{
    public class CommunicationState
    {
        public CommunicationState()
        {
            LastMessageReceivedTicks = DateTime.Now.Ticks;
            MessageContentBuilder = new StringBuilder();
        }

        public byte[] Buffer { get; } = new byte[Constants.BufferSize];
        private StringBuilder MessageContentBuilder { get; }
        public long LastMessageReceivedTicks { get; set; }

        public (IEnumerable<string> messageList, bool hasEtbByte) SplitMessages(int bytesRead, int socketId)
        {
            MessageContentBuilder.Append(Encoding.ASCII.GetString(Buffer, 0, bytesRead));
            var content = MessageContentBuilder.ToString();
            Debug.WriteLine($"Socket {socketId}:\nData : {content}");

            var messages = new string[0];
            var hasEtbByte = content.IndexOf(Constants.EtbByte) > -1;

            if (hasEtbByte)
            {
                messages = content.Split(Constants.EtbByte);
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