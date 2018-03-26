namespace GameMaster.Delays
{
    public interface IDelayed
    {
        double GetDelay(ActionCosts actionCosts);
    }
}