using System.Threading;

namespace Communication.Client
{
    public interface IConnector
    {
        ITcpConnection TcpConnection { get; set; }
        ManualResetEvent ConnectFinalized { get; set; }
        void Connect();
    }
}