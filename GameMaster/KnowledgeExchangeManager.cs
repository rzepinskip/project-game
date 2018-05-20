using System;
using System.Collections.Generic;
using System.Linq;
using Common.Interfaces;

namespace GameMaster
{
    public class KnowledgeExchangeManager : IKnowledgeExchangeManager
    {
        private readonly Dictionary<int, List<ExchangeState>> _initiatorsExchanges;
        private readonly Dictionary<int, List<ExchangeState>> _subjectsExchanges;

        public KnowledgeExchangeManager()
        {
            _initiatorsExchanges = new Dictionary<int, List<ExchangeState>>();
            _subjectsExchanges = new Dictionary<int, List<ExchangeState>>();
        }

        public void AssignSubjectToInitiator(int initiatorId, int subjectId)
        {
            var exchange = new ExchangeState(initiatorId: initiatorId, subjectId: subjectId);
            
            if (!_initiatorsExchanges.ContainsKey(initiatorId))
                _initiatorsExchanges.Add(initiatorId, new List<ExchangeState>());
            if (!_subjectsExchanges.ContainsKey(subjectId))
                _subjectsExchanges.Add(subjectId, new List<ExchangeState>());

            _initiatorsExchanges[initiatorId].Add(exchange);
            _subjectsExchanges[subjectId].Add(exchange);
        }

        public bool IsExchangeInitiator(int dataSenderId, int receiverId)
        {
            if (_initiatorsExchanges.ContainsKey(dataSenderId))
            {
                return _initiatorsExchanges[dataSenderId].Any(e => e.SubjectId == receiverId);
            }

            return false;
        }

        public IMessage FinalizeExchange(int initiatorId, int subjectId)
        {
            Console.WriteLine($"{initiatorId} with {subjectId} succesful exchange");
            var exchange = _initiatorsExchanges[initiatorId].First(e => e.SubjectId == subjectId);
            _initiatorsExchanges[initiatorId].Remove(exchange);
            if (!_subjectsExchanges[subjectId].Remove(exchange))
                throw new NotSupportedException();
            return exchange.InitiatorData;
        }

        public void AttachDataToInitiator(IMessage responseWithData, int initiatorId, int subjectId)
        {
            _initiatorsExchanges[initiatorId]
                .First(e => e.InitiatorId == initiatorId 
                            && e.SubjectId == subjectId)
                .InitiatorData = responseWithData;
        }

        public bool HasMatchingInitiatorWithData(int senderId, int initiatorId)
        {
            if (!_subjectsExchanges.ContainsKey(senderId))
                return false;

            return _subjectsExchanges[senderId].Any( e => e.InitiatorId == initiatorId && e.InitiatorData != null);
        }


        public void HandleExchangeRejection(int subjectId, int initiatorId)
        {
            _subjectsExchanges[subjectId].Remove(_subjectsExchanges[subjectId].First(e => e.InitiatorId == initiatorId && e.SubjectId == subjectId));
            _initiatorsExchanges[initiatorId].Remove(_initiatorsExchanges[initiatorId].First(e => e.InitiatorId == initiatorId && e.SubjectId == subjectId));
        }
    }
}