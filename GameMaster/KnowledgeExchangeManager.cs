using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Interfaces;

namespace GameMaster
{
    public class KnowledgeExchangeManager : IKnowledgeExchangeManager
    {
        private readonly Object _managerLock;
        private readonly Dictionary<int, List<ExchangeState>> _initiatorsExchanges;
        private readonly Dictionary<int, List<ExchangeState>> _subjectsExchanges;
        private readonly TimeSpan _knowledgeExchangeDelay;

        public KnowledgeExchangeManager(double knowledgeExchangeDelay)
        {
            _initiatorsExchanges = new Dictionary<int, List<ExchangeState>>();
            _subjectsExchanges = new Dictionary<int, List<ExchangeState>>();
            _knowledgeExchangeDelay = TimeSpan.FromMilliseconds(knowledgeExchangeDelay);
            _managerLock = new Object();
        }

        public void AssignSubjectToInitiator(int initiatorId, int subjectId)
        {
            var exchange = new ExchangeState(initiatorId: initiatorId, subjectId: subjectId);
            lock (_managerLock)
            {
                if (!_initiatorsExchanges.ContainsKey(initiatorId))
                    _initiatorsExchanges.Add(initiatorId, new List<ExchangeState>());
                if (!_subjectsExchanges.ContainsKey(subjectId))
                    _subjectsExchanges.Add(subjectId, new List<ExchangeState>());

                _initiatorsExchanges[initiatorId].Add(exchange);
                _subjectsExchanges[subjectId].Add(exchange);
            }
        }

        public bool IsExchangeInitiator(int dataSenderId, int receiverId)
        {
            var result = false;
            lock (_managerLock)
            {
                result = _initiatorsExchanges.ContainsKey(dataSenderId) && _initiatorsExchanges[dataSenderId].Any(e => e.SubjectId == receiverId);
            }

            return result;
        }

        public IMessage DelayedFinalizeExchange(int initiatorId, int subjectId)
        {
            //Console.WriteLine($"{initiatorId} with {subjectId} succesful exchange");
            ExchangeState exchange;
            lock (_managerLock)
            {
                exchange = _initiatorsExchanges[initiatorId].First(e => e.SubjectId == subjectId);
                _initiatorsExchanges[initiatorId].Remove(exchange);
                if (!_subjectsExchanges[subjectId].Remove(exchange))
                    throw new NotSupportedException();
            }
            Task.Delay(_knowledgeExchangeDelay);
            // Console.WriteLine($"Finalize exchange between player {initiatorId} and {subjectId}");
            return exchange.InitiatorData;
        }

        public void AttachDataToInitiator(IMessage responseWithData, int initiatorId, int subjectId)
        {

            lock (_managerLock)
            {
                _initiatorsExchanges[initiatorId]
                    .First(e => e.InitiatorId == initiatorId 
                                && e.SubjectId == subjectId)
                    .InitiatorData = responseWithData;
            }
        }

        public bool HasMatchingInitiatorWithData(int senderId, int initiatorId)
        {
            var result = false;
            lock (_managerLock)
            {
                if (_subjectsExchanges.ContainsKey(senderId))
                    result = _subjectsExchanges[senderId].Any( e => e.InitiatorId == initiatorId && e.InitiatorData != null);
            }

            return result;
        }


        public void HandleExchangeRejection(int subjectId, int initiatorId)
        {
            lock (_managerLock)
            {
                _subjectsExchanges[subjectId].Remove(_subjectsExchanges[subjectId].First(e => e.InitiatorId == initiatorId && e.SubjectId == subjectId));
                _initiatorsExchanges[initiatorId].Remove(_initiatorsExchanges[initiatorId].First(e => e.InitiatorId == initiatorId && e.SubjectId == subjectId));
            }
        }
    }
}