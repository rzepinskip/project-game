namespace Player.Strategy.StateInfo
{
    public abstract class BaseInfo : IExceptionContentProvider
    {
        public string GetExceptionInfo()
        {
            return ToString();
        }

        public string ToLog()
        {
            return ToString();
        }
    }
}