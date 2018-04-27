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
        public StringBuilder Sb { get; }
        public long LastMessageReceivedTicks { get; set; }

        public CommunicationStateObject()
        {
            LastMessageReceivedTicks = DateTime.Now.Ticks;
            Sb = new StringBuilder();
        }

        public (IEnumerable<string> messageList, bool hasEtbByte) SplitMessages(int bytesRead, int id)
        {
            Sb.Append(Encoding.ASCII.GetString(Buffer, 0, bytesRead));
            var content = Sb.ToString();
            Debug.WriteLine("Read {0} bytes from socket {1} . \n Data : {2}",
                content.Length, id, content);

            var messages = new string[0];
            var hasEtbByte = content.IndexOf(CommunicationStateObject.EtbByte) > -1;
            
            if (hasEtbByte)
            {
                messages = content.Split(CommunicationStateObject.EtbByte);
                var numberOfMessages = messages.Length;
                var wholeMessages = string.IsNullOrEmpty(messages[numberOfMessages - 1]);

                for (var i = 0; i < messages.Length - 1; ++i)
                {
                    var message = messages[i];
                    LastMessageReceivedTicks = DateTime.Today.Ticks;
                }

                Sb.Clear();

                if (!wholeMessages)
                    Sb.Append(messages[numberOfMessages - 1]);
            }

            return (messages.Take(messages.Length - 1), hasEtbByte);
        }
    }
}
