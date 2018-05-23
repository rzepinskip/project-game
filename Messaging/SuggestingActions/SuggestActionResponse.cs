using System.Collections.Generic;
using System.Xml.Serialization;
using Common.BoardObjects;

namespace Messaging.SuggestingActions
{
    /// <summary>
    ///     There is such an element defined in schema, but a copy of SuggestAction. Thus, useless.
    /// </summary>
    [XmlType(XmlRootName)]
    public class SuggestActionResponse : SuggestAction
    {
        public new const string XmlRootName = "SuggestActionResponse";

        public SuggestActionResponse(int playerId, int senderPlayerId, IEnumerable<TaskField> taskFields,
            IEnumerable<GoalField> goalFields) : base(playerId, senderPlayerId, taskFields, goalFields)
        {
        }

        protected SuggestActionResponse()
        {
        }
    }
}