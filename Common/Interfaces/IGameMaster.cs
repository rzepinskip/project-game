namespace Common.Interfaces
{
    public interface IGameMaster
    {
        (DataFieldSet data, bool isGameFinished) EvaluateAction(ActionInfo.ActionInfo actionInfo);
    }
}