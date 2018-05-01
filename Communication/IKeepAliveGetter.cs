using System.Collections.Generic;

namespace Communication
{
    public interface IKeepAliveGetter
    {
        IEnumerable<ITcpConnection> Get();
    }
}
