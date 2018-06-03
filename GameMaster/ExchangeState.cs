using Common.Interfaces;

namespace GameMaster
{
    public class ExchangeState
    {
        public ExchangeState(int initiatorId, int subjectId)
        {
            InitiatorId = initiatorId;
            SubjectId = subjectId;
        }

        public int InitiatorId { get; set; }
        public int SubjectId { get; set; }
        public IMessage InitiatorData { get; set; }
    }
}