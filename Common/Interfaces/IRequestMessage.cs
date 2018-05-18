using System;

namespace Common.Interfaces
{
    public interface IRequestMessage : IMessage
    {
        Guid PlayerGuid { get; set; }

        ActionInfo.ActionInfo GetActionInfo();
    }
}