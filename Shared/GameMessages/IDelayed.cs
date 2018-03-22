namespace Shared.GameMessages
{
    public interface IDelayed
    {
        double GetDelay(ActionCosts actionCosts);
    }
}