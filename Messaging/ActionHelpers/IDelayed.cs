namespace Messaging.ActionHelpers
{
    public interface IDelayed
    {
        double GetDelay(ActionCosts actionCosts);
    }
}