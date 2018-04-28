using System.Threading;

namespace Communication
{
    public interface IConnecter
    {
        ICommunicationTool ClientTcpCommunicationTool { get; set; }
        ManualResetEvent ConnectDoneForSend { get; set; }
        void Connect();
    }
}