using System;

namespace Common.ActionInfo
{
    public class ActionInfo
    {
        public ActionInfo(Guid playerGuid)
        {
            PlayerGuid = playerGuid;
        }

        public Guid PlayerGuid { get; }
    }
}