namespace Common.ActionInfo
{
    public class ActionInfo
    {
        public ActionInfo(string playerGuid)
        {
            PlayerGuid = playerGuid;
        }

        public string PlayerGuid { get; }
    }
}