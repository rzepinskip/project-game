using System;
using System.Threading;
using Common.Interfaces;

namespace Communication.Client
{
    public interface IConnector
    {
        ITcpConnection TcpConnection { get; set; }
        ManualResetEvent ConnectFinalized { get; set; }
        void SetIncomingMessageHandler(Action<IMessage> handler);
        void Connect();
    }
}