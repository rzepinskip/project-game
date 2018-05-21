namespace Common.Interfaces
{
    public interface IKnowledgeExchangeManager
    {
        void AssignSubjectToInitiator(int initiatorId, int subjectId);
        void AttachDataToInitiator(IMessage responseWithData, int initiatorId, int subjectId);
        bool HasMatchingInitiatorWithData(int senderId, int initiatorId);
        void HandleExchangeRejection(int subjectId, int initiatorId);
        bool IsExchangeInitiator(int senderId, int receiverId);
        IMessage DelayedFinalizeExchange(int initiatorId, int subjectId);
    }
}