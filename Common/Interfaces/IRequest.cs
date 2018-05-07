using System;

namespace Common.Interfaces
{
    public interface IRequest : IMessage, ILoggable
    {
        Guid PlayerGuid { get; set; }

        ActionInfo.ActionInfo GetActionInfo();
    }
}