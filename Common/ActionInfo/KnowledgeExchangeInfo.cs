using System;
using System.Collections.Generic;
using System.Text;

namespace Common.ActionInfo
{
    public class KnowledgeExchangeInfo : ActionInfo
    {
        public KnowledgeExchangeInfo(Guid initiatorGuid, int subjectId) : base(initiatorGuid)
        {
            SubjectId = subjectId;
        }

        public int SubjectId { get; set; }
    }
}