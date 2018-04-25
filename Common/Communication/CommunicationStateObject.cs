using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Common.Interfaces;

namespace Common.Communication
{
    public class CommunicationStateObject
    {
        public const int BufferSize = 1024;
        public const char EtbByte = (char) 23;
        public byte[] Buffer { get; } = new byte[BufferSize];
        public StringBuilder Sb { get; } = new StringBuilder();
        public long LastMessageReceivedTicks { get; set; }

        public CommunicationStateObject()
        {
            LastMessageReceivedTicks = DateTime.Now.Ticks;
        }

        public (IEnumerable<string> messageList, bool isLastMessageRead) SplitMessages(int bytesRead)
        {
            Sb.Append(Encoding.ASCII.GetString(Buffer, 0, bytesRead));
            var content = Sb.ToString();
            var messages = new string[0];
            var wholeMessages = true;

            if (content.IndexOf(CommunicationStateObject.EtbByte) > -1)
            {
                messages = content.Split(CommunicationStateObject.EtbByte, StringSplitOptions.RemoveEmptyEntries);
                var numberOfMessages = messages.Length;
                wholeMessages = string.IsNullOrEmpty(messages[numberOfMessages - 1]);

                for (var i = 0; i < numberOfMessages - 1; ++i)
                {
                    var message = messages[i];
                    Debug.WriteLine("Read {0} bytes from socket. \n Data : {1}",
                        message.Length, message);
                    LastMessageReceivedTicks = DateTime.Today.Ticks;
                }

                Sb.Clear();

                if (!wholeMessages)
                    Sb.Append(messages[numberOfMessages - 1]);
            }

            return (messages.ToList(), wholeMessages);
        }
    }
}
