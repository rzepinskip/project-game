using System.Threading;

namespace Communication
{
    public interface IConnector
    {
        ITcpConnection TcpConnection { get; set; }
        ManualResetEvent ConnectFinalized { get; set; }
        void Connect();
    }
}