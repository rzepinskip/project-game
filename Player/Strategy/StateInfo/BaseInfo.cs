namespace Player.Strategy.StateInfo
{
    public abstract class BaseInfo: IExceptionContentProvider 
    {
        public string getExceptionInfo()
        {
            return ToString();
        }
        public string ToLog()
        {
            return this.ToString();
        }
    }
}