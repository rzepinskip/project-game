using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Common.BoardObjects;
using Common.Interfaces;

namespace Messaging.SuggestingActions
{
    [XmlType(XmlRootName)]
    public class SuggestAction : BetweenPlayersMessage
    {
        public const string XmlRootName = "SuggestAction";

        public SuggestAction(int playerId, int senderPlayerId, IEnumerable<TaskField> taskFields,
            IEnumerable<GoalField> goalFields) : base(playerId, senderPlayerId)
        {
            TaskFields = taskFields?.ToArray();
            GoalFields = goalFields?.ToArray();
        }

        protected SuggestAction()
        {
            TaskFields = new TaskField[0];
            GoalFields = new GoalField[0];
        }

        public TaskField[] TaskFields { get; }

        public GoalField[] GoalFields { get; }

        [XmlAttribute("gameId")] public int GameId { get; set; }

        [XmlIgnore] public Guid? PlayerGuid { get; set; }

        [XmlAttribute("playerGuid")]
        public Guid PlayerGuidValue
        {
            get
            {
                if (PlayerGuid != null)
                {
                    return PlayerGuid.Value;
                }

                throw new InvalidOperationException();
            }
            set => PlayerGuid = value;
        }

        [XmlIgnore] public bool PlayerGuidValueSpecified => PlayerGuid.HasValue;

        public override IMessage Process(IGameMaster gameMaster)
        {
            return null;
        }

        public override void Process(IPlayer player)
        {
        }

        public override void Process(ICommunicationServer cs, int id)
        {
        }
    }
}
