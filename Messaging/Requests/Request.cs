using System.Xml.Serialization;
using Common.Interfaces;
using Messaging.ActionHelpers;
using Messaging.Responses;

namespace Common.GameMessages
{
    [XmlRoot(Namespace = "https://se2.mini.pw.edu.pl/17-results/")]
    public abstract class Request : ILoggable, IDelayed
    {
        [XmlAttribute]
        public int PlayerId { get; set; }

        public string PlayerGuid { get; set; }

        [XmlAttribute]
        public int GameId { get; set; }

        public abstract double GetDelay(ActionCosts actionCosts);

        public abstract ActionLog ToLog(int playerId, PlayerInfo playeInfo);

        public abstract Response Execute(IBoard board);
    }
}