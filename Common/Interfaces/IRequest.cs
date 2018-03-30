namespace Common.Interfaces
{
    public interface IRequest : IMessage, ILoggable
    {
        string PlayerGuid { get; set; }

        ActionInfo.ActionInfo GetActionInfo();
    }
}