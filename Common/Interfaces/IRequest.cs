namespace Common.Interfaces
{
    public interface IRequest : IMessage
    {
        string PlayerGuid { get; set; }

        ActionInfo.ActionInfo GetActionInfo();
    }
}